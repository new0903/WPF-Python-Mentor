using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfAppPythone.AppData.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfAppPythone.PythonCompiler
{
    public class PythoneCompiler 
    {
        private string Version=string.Empty;
        public static bool IsInstall = false;
        private readonly string _scriptName = Path.Combine(Directory.GetCurrentDirectory(), "ForTestScript.py");
        private string _pythonExePath = "python";
        private ProcessStartInfo compiler;
        public PythoneCompiler() {


         //   _=InitPython();
            compiler = new ProcessStartInfo()
            {
                FileName = _pythonExePath,
                Arguments = $"\"{_scriptName}\"",
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false
            };
        }
        /*
         * переделать код полностью
         * python -c "import sys; print(sys.executable)" путь к python
         * вывод C:\Python313\python.exe
         * python --version версия pythone
         * через cmd 
         * & C:/Python313/python.exe "d:/Visual Studio 2010/WPF/Python/bin/Debug/net8.0-windows/ForTestScript.py"
         * 
         абсолютные пути так быть не должн 
         ошибки при попытке компиляции кода test=int(input())
 сообщение ошибки ex.Message=An error occurred trying to start process 'C:\Python311\python.exe' with working directory 'D:\Visual Studio 2010\WPF\Python\bin\Debug\net8.0-windows'. Не удается найти указанный файл.
 ex.Source=System.Diagnostics.Process
         */


        public async Task<bool> InitPython()
        {
            try
            {
                var ct = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                var compilerStart= new ProcessStartInfo()
                {
                    FileName = _pythonExePath,
                    Arguments = "--version",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false
                };
                using var process = Process.Start(compilerStart);

                if (process==null)
                {

                    return false;
                }
                var outputProcess =  process.StandardOutput.ReadToEndAsync(ct.Token);
                var errorProcess =  process.StandardError.ReadToEndAsync(ct.Token);

                await process.WaitForExitAsync(ct.Token);
                string output = await outputProcess;
                string error = await errorProcess;
                if (!string.IsNullOrEmpty(error))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(output)) {
                    return false;
                }
                Version = output;
                IsInstall= true;

                return true;
            }
            catch( OperationCanceledException ex)
            {

                MessageBox.Show($"При проверке установлен ли python на этом устройстве возникли ошибки\nСообщение об ошибке: {ex.Message}");
            }
            catch (Exception ex)
            {

                MessageBox.Show($"При проверке установлен ли python на этом устройстве возникли ошибки\nСообщение об ошибке: {ex.Message}");
                
            }
            return false;
        }


 

        public async Task<string[]?> StartTestCode(Quest quest,string PCode)
        {
            if (!IsInstall)
            {
                return [string.Empty,"Python не установлен, вы не можете выполнить эту задачу без python"];
            }

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            try
            {
                if (quest.QType != AppData.Models.Enums.QuestType.Code)
                {
                    MessageBox.Show("Как сюда попала задача не для кода");
                    return new string[] { string.Empty, "Как сюда попала задача не для кода" };
                }

                await File.WriteAllTextAsync(_scriptName, PCode, cts.Token);

                using var process = Process.Start(compiler);

                if (process == null)
                {
                    MessageBox.Show("ошибки какие то process = null");
                    return new string[] { string.Empty, "ошибки какие то process = null" };
                }
                if (quest.OptionJSON.Length > 0)
                {
                    foreach (var item in quest.OptionJSON)
                    {
                        await process.StandardInput.WriteLineAsync(item);

                    }
                    await process.StandardInput.FlushAsync( cts.Token);
                    process.StandardInput.Close();
                }

                var outputProcess = process.StandardOutput.ReadToEndAsync(cts.Token);
                var errorProcess = process.StandardError.ReadToEndAsync(cts.Token);
                await process.WaitForExitAsync(cts.Token);
                string output = await outputProcess;
                string error = await errorProcess;
                return new string[] { output, error };
            }
            catch (OperationCanceledException)
            {
                return new string[] { string.Empty, "Время выполнения кода превысило лимит (60 секунд)." };
            }
            catch (Exception ex )
            {

                return new string[] { string.Empty, $"ошибки при попытке компиляции кода {PCode}\n сообщение ошибки ex.Message={ex.Message}\n ex.Source={ex.Source}" };
            }

        }
    }
}
