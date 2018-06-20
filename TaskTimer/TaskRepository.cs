using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TaskTimer.Domain;

namespace TaskTimer
{
    public class TaskRepository: ITaskRepository
    {
        private const string TaskHistoryJsonFile = @"c:\temp\log.json";

        public void SaveTasks(IList<TimerTask> list)
        {
            var listJson = JsonConvert.SerializeObject(list,Formatting.Indented);
            File.WriteAllText(TaskHistoryJsonFile, listJson);
        }

        public IList<TimerTask> LoadTasks()
        {
            if (File.Exists(TaskHistoryJsonFile))
            {
                var readAllText = File.ReadAllText(TaskHistoryJsonFile);
                var deserializeObject = JsonConvert.DeserializeObject<List<TimerTask>>(readAllText);
                return deserializeObject;
            }
            else return new List<TimerTask>();
        }
    }
}