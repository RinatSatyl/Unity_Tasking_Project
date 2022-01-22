﻿// Класс для контролирования UI экрана списка UI

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PieChart.ViitorCloud;

namespace Tasking
{
    public class TaskingUIManager : MonoBehaviour
    {
        [SerializeField] GameObject taskUIPrefab;
        [SerializeField] GameObject taskUIList;
        [SerializeField] RectTransform contentTransform;
        [SerializeField] GridLayoutGroup gridLayoutGroup;
        [SerializeField] GameObject pieChartPrefab;
        [SerializeField] GameObject barGraphPrefab;
        [SerializeField] GameObject hideChartsButton;

        // Ссылки на созданные объекты статистики
        GameObject pieChartObject;
        GameObject barGraphObject;
        // Ссылки на компоненты статистик
        PieChart.ViitorCloud.PieChart pieChart;
        BarGraph.VittorCloud.BarGraphGenerator barGraph;

        // Список UI объектов задач на экране
        List<TaskUI> taskUIObjects = new List<TaskUI>();

        // Статичная ссылка на этот объект
        public static TaskingUIManager Instance;

        // Список цветов для отображения в зависимости от статуса
        private Dictionary<TaskingStatus, Color> taskColor = new Dictionary<TaskingStatus, Color>();
        // Список читаемых названий статуса в зависимости от статуса
        private Dictionary<TaskingStatus, string> taskStatusName = new Dictionary<TaskingStatus, string>();

        // Публичная read-only ссылка на список цветов
        public Dictionary<TaskingStatus, Color> TaskColor { get { return taskColor; } }
        // Публичная read-only список названий для статусов
        public Dictionary<TaskingStatus, string> TaskStatusName { get { return taskStatusName; } }

        struct AssigneeStatisic
        {
            public string assigneeName;
            public float[] tasksStatus;

            public AssigneeStatisic(string assigneeName, float[] tasksStatus)
            {
                this.assigneeName = assigneeName;
                this.tasksStatus = tasksStatus;
            }
        }

        private void Start()
        {
            Instance = this;

            // Пополнить списки для отображения
            taskColor.Add(TaskingStatus.COMPLETED, Color.green);
            taskColor.Add(TaskingStatus.IN_PROGRESS, Color.yellow);
            taskColor.Add(TaskingStatus.OPEN, Color.white);
            taskColor.Add(TaskingStatus.REVIEWING, new Color(0.9f, 0.47f, 0.25f));
            
            taskStatusName.Add(TaskingStatus.COMPLETED, "Закончено");
            taskStatusName.Add(TaskingStatus.IN_PROGRESS, "В прогрессе");
            taskStatusName.Add(TaskingStatus.OPEN, "Открыто");
            taskStatusName.Add(TaskingStatus.REVIEWING, "Рассматривается");
        }
        // Метод для очистки UI, при полной очистке списка задач
        public void OnTasksClear()
        {
            // Очистить список ссылок на UI объекты задач
            taskUIObjects.Clear();
            // Удалить сами объекты задач
            if (taskUIList.transform.transform.childCount > 0)
            {
                for(int i = 0; i < taskUIList.transform.transform.childCount; i++)
                {
                    Destroy(taskUIList.transform.transform.GetChild(i).gameObject);
                }
            }
            // Обновить зону прокрутки
            UpdateScrollAreaSize();
            // Убрать объекты с статистикой
            UpdateCharts();
        }
        // Метод для добавления UI объекта задачи
        public void OnTaskCreated(TaskingTask newTask)
        {
            // Создать объект
            GameObject newTaskUI = Instantiate(taskUIPrefab, taskUIList.transform);
            // Добавить его в список объектов
            taskUIObjects.Add(newTaskUI.GetComponent<TaskUI>());
            // Задать отображаемую информацию
            newTaskUI.GetComponent<TaskUI>().SetInformation(newTask);

            // Обновить зону прокрутки
            UpdateScrollAreaSize();
            // Обновить/Создать объекты статистики
            UpdateCharts();
        }
        // Метод для удаления объекта UI задачи
        public void OnTaskDeleted(string taskId)
        {
            foreach(TaskUI thisTaskUI in taskUIObjects)
            {
                if (thisTaskUI.TaskID == taskId)
                {
                    // Удалить UI объект задачи
                    Destroy(thisTaskUI.gameObject);
                    // Удалить ссылку со списка
                    taskUIObjects.Remove(thisTaskUI);

                    // Обновить зону прокрутки и обновить/Создать объекты статистики
                    UpdateScrollAreaSize();
                    UpdateCharts();
                    return;
                }
            }
        }
        // Метод для обновления UI элементов
        public void OnTaskUpdated(TaskingTask newTask)
        {
            // Обновить объекты статистики
            UpdateCharts();
        }
        public void SwitchGraphsState()
        {
            if (pieChartObject != null)
            {
                pieChartObject.SetActive(!pieChartObject.activeSelf);
                barGraphObject.SetActive(!barGraphObject.activeSelf);
            }
        }
        // Метод для обновления размера объекта прокрутки.
        void UpdateScrollAreaSize()
        {
            contentTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x, gridLayoutGroup.cellSize.y * taskUIObjects.Count);
        }
        // Метод для обновления/создания объектов статистики
        void UpdateCharts()
        {
            // Удалить объекты статистики если они были созданы
            if (pieChartObject != null)
            {
                Destroy(pieChartObject);
            }
            if (barGraphObject != null)
            {
                Destroy(barGraphObject);
            }

            hideChartsButton.SetActive(false);

            // Если список задач не пуст
            if (taskUIObjects.Count > 0)
            {
                hideChartsButton.SetActive(true);

                // Очистить данные статистики
                float[] statusStatistic = new float[TaskingManager.Instance.PossibleTaskStatuses];

                // Заполить данные статистики
                foreach(TaskUI thisTask in taskUIObjects)
                {
                    statusStatistic[thisTask.TaskStatus]++;
                }

                // Создать объекты статистики, получить ссылки на объект и компонент статистики
                pieChartObject = Instantiate(pieChartPrefab, gameObject.transform.parent.transform);
                barGraphObject = Instantiate(barGraphPrefab, gameObject.transform.parent.transform);
                pieChart = pieChartObject.GetComponent<PieChart.ViitorCloud.PieChart>();
                barGraph = barGraphObject.GetComponent<BarGraph.VittorCloud.BarGraphGenerator>();

                // Задать размер графы
                barGraph.segmentSizeOnXaxis = 4f;
                barGraph.segmentSizeOnYaxis = 0.4f;

                // Создать список цветов задач для объктов статистик
                Color[] customColors = new Color[TaskingManager.Instance.PossibleTaskStatuses];

                for (int i = 0; i < TaskingManager.Instance.PossibleTaskStatuses; i++)
                {
                    customColors[i] = taskColor[(TaskingStatus)i];
                }

                // Логика для PieChart
                pieChart.customColors = customColors; // Передать цвета
                pieChart.segments = TaskingManager.Instance.PossibleTaskStatuses; // Задать количество сегментов
                pieChart.Data = statusStatistic; // Передать статистику
                
                // Локига для barGraph

                // Создать список с данными для графы
                List<BarGraph.VittorCloud.BarGraphDataSet> barGraphDataSet = new List<BarGraph.VittorCloud.BarGraphDataSet>();

                // Узнать какие пользователи существуют в списке
                List<AssigneeStatisic> assigneeList = new List<AssigneeStatisic>();

                foreach (TaskingTask task in TaskingManager.Instance.TaskList)
                {
                    if (assigneeList.Exists(x => x.assigneeName == task.assignee) == false)
                    {
                        AssigneeStatisic thisAssignee = new AssigneeStatisic(task.assignee, new float[TaskingManager.Instance.PossibleTaskStatuses]);
                        assigneeList.Add(thisAssignee);
                    }
                }

                // Составить статистику по задачам пользователей в списке
                foreach (TaskingTask task in TaskingManager.Instance.TaskList)
                {
                    AssigneeStatisic thisAssignee = assigneeList.Find(x => x.assigneeName == task.assignee);
                    thisAssignee.tasksStatus[task.status]++;
                }

                for (int i = 0; i < TaskingManager.Instance.PossibleTaskStatuses; i++)
                {
                    BarGraph.VittorCloud.BarGraphDataSet newData = new BarGraph.VittorCloud.BarGraphDataSet();

                    // Передать цвет задачи
                    newData.barColor = taskColor[(TaskingStatus)i];
                    newData.GroupName = string.Empty;

                    newData.ListOfBars = new List<BarGraph.VittorCloud.XYBarValues>();

                    foreach (AssigneeStatisic thisAssignee in assigneeList)
                    {
                        BarGraph.VittorCloud.XYBarValues barPos = new BarGraph.VittorCloud.XYBarValues()
                        {
                            XValue = thisAssignee.assigneeName,
                            YValue = thisAssignee.tasksStatus[i],
                        };

                        newData.ListOfBars.Add(barPos);
                    }

                    Debug.Log(assigneeList.Count);

                    // Добавить данные в графу
                    barGraphDataSet.Add(newData);
                }

                // Сгенерировать графу по переданным данным
                barGraph.GeneratBarGraph(barGraphDataSet);
            }
        }
    }
}