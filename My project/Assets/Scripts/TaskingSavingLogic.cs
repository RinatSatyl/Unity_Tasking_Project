// Класс для сохранения/загрузки задач в списке задач

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Tasking
{
    public class TaskingSavingLogic : MonoBehaviour
    {
        private const string SAVED_TASKS_PLAYERPREFS_KEY = "Tasks_Tasks";

        // Метод для конвертирования списка задач в один json файл
        private string FromTasksToJson()
        {
            // Создать массив размером с количеством задач в списке, для хранения json информации каждой задачи
            string[] jsonInformationArray = new string[TaskingManager.Instance.TaskList.Count];

            int count = 0;
            foreach(TaskingTask thisTask in TaskingManager.Instance.TaskList)
            {
                // Трансформировать TaskingTask объект с информацией в json и записать полученный string в массив
                jsonInformationArray[count] = JsonConvert.SerializeObject(thisTask, Formatting.Indented);
                // Перейти к следующей ячейке
                count++;
            }

            // Вернуть один Json string со всей информацией
            return JsonConvert.SerializeObject(jsonInformationArray, Formatting.Indented);
        }
        // Метод для конвертирования json информации в задачи, добавления загруженых задач в список задач
        private void FromJsonToTasks(string json)
        {
            // Очистить список задач
            TaskingManager.Instance.ClearTaskList();

            // Разбить полученный json файл на массив с отдельными json задачи
            string[] jsonInformationArray = JsonConvert.DeserializeObject<string[]>(json);

            for (int i = 0; i < jsonInformationArray.Length; i++)
            {
                // Трансформировать полученую json инфо в TaskingTask объект и добавить полученный объект задачи в список задач
                TaskingManager.Instance.CreateTask(JsonConvert.DeserializeObject<TaskingTask>(jsonInformationArray[i]));
            }
        }

        // Метод для сохранения задач в формате json в PlayerPrefs
        public void Save()
        {
            string tasksJson = FromTasksToJson();
            Debug.Log("Сохранено - " + tasksJson);
            PlayerPrefs.SetString(SAVED_TASKS_PLAYERPREFS_KEY, tasksJson);
        }
        // Метод для загрузки списка задач с PlayerPrefs
        public void Load()
        {
            string tasksJson = PlayerPrefs.GetString(SAVED_TASKS_PLAYERPREFS_KEY);
            Debug.Log("Загружено - " + tasksJson);
            FromJsonToTasks(tasksJson);
        }
    }
}