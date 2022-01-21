// Класс для сохранения/загрузки задач в списке задач

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Tasking
{
    public class TaskSavingManager : MonoBehaviour
    {
        private const string SAVED_TASKS_PLAYERPREFS_KEY = "Tasks_Tasks";

        // Метод для конвертирования списка задач в один json файл
        private string FromTasksToJson()
        {
            // Создать массив размером с количеством задач в списке для хранения json информации каждой задачи
            string[] jsonInformationArray = new string[TaskManager.Instance.TaskList.Count];

            int count = 0;
            foreach(TaskManager.TaskingTask thisTask in TaskManager.Instance.TaskList)
            {
                // Трансформировать TaskingTask объект с информацией в json
                // Записать полученный string в массив
                jsonInformationArray[count] = JsonConvert.SerializeObject(thisTask);
                // Перейти к следующей ячейке
                count++;
            }

            // Вернуть один Json файл со всей информацией
            return JsonConvert.SerializeObject(jsonInformationArray);
        }
        // Метод для конвертирования json информации в список задач для TaskManager
        private void FromJsonToTasks(string json)
        {
            // Почистить список задач
            TaskManager.Instance.ClearTaskList();

            // Разбить полученный json файл на массив с отдельными json задачами
            string[] jsonInformationArray = JsonConvert.DeserializeObject<string[]>(json);


            foreach (string jsonTaskInformation in jsonInformationArray)
            {
                // Трансформировать полученую json инфо в TaskingTask объект.
                TaskManager.TaskingTask receivedTask = JsonConvert.DeserializeObject<TaskManager.TaskingTask>(jsonTaskInformation);
                // Добавить полученный объект задачи в новый список задач
                TaskManager.Instance.CreateTask(receivedTask);
            }
        }

        // Метод для сохранения задач в формате json в PlayerPrefs
        public void Save()
        {
            string tasksJson = FromTasksToJson();
            Debug.Log(tasksJson);
            PlayerPrefs.SetString(SAVED_TASKS_PLAYERPREFS_KEY, tasksJson);
        }
        // Метод для загрузки списка задач с PlayerPrefs
        public void Load()
        {
            string tasksJson = PlayerPrefs.GetString(SAVED_TASKS_PLAYERPREFS_KEY);
            Debug.Log(tasksJson);
            FromJsonToTasks(tasksJson);
        }
    }
}