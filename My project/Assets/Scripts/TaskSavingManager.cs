// Класс для сохранения/загрузки задач в списке задач

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                jsonInformationArray[count] = JsonUtility.ToJson(thisTask);
                // Перейти к следующей ячейке
                count++;
            }

            // Вернуть один Json файл со всей информацией
            return JsonUtility.ToJson(jsonInformationArray);
        }
        // Метод для конвертирования json информации в список задач для TaskManager
        private List<TaskManager.TaskingTask> FromJsonToTasks(string json)
        {
            // Почистить список задач
            TaskManager.Instance.ClearTaskList();

            // Разбить полученный json файл на массив с отдельными json задачами
            string[] jsonInformationArray = JsonUtility.FromJson<string[]>(json);

            // Создать временный новый список задач
            List<TaskManager.TaskingTask> newTaskList = new List<TaskManager.TaskingTask>();

            foreach (string jsonTaskInformation in jsonInformationArray)
            {
                // Трансформировать полученую json инфо в TaskingTask объект.
                TaskManager.TaskingTask receivedTask = JsonUtility.FromJson<TaskManager.TaskingTask>(jsonTaskInformation);
                // Добавить полученный объект задачи в новый список задач
                newTaskList.Add(receivedTask);
            }

            return newTaskList;
        }

        // Метод для сохранения задач в формате json в PlayerPrefs
        public void Save()
        {
            string tasksJson = FromTasksToJson();
            PlayerPrefs.SetString(SAVED_TASKS_PLAYERPREFS_KEY, tasksJson);
        }
        // Метод для загрузки списка задач с PlayerPrefs
        public void Load()
        {
            string tasksJson = PlayerPrefs.GetString(SAVED_TASKS_PLAYERPREFS_KEY);
            FromJsonToTasks(tasksJson);
        }
    }
}