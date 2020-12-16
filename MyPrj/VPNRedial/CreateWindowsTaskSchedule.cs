using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler;
namespace VPNRedial
{
    public class CreateWindowsTaskSchedule
    {

        #region 创建一个windows任务计划

         ////1.连接TaskSchedulerClass
         //   TaskSchedulerClass scheduler = new TaskSchedulerClass();
         //   scheduler.Connect(
         //       "",//电脑名或者IP
         //       "",//用户名
         //       "", //域名
         //       "");//密码

         //   //2.获取计划任务文件夹(参数：选中计划任务后'常规'中的'位置'，根文件夹为"\\")
         //   ITaskFolder folder = scheduler.GetFolder("\\");

         //   //3.例：获取名称为"TaskA"的计划任务
         //   IRegisteredTask task = folder.GetTask("TaskA");
         //   //运行(带参数)
         //   IRunningTask runningTask = task.Run(null);
         //   //停止(参数为预留参数，只能填0)
         //   task.Stop(0);
         //   //禁用
         //   task.Enabled = false;
         //   //启用
         //   task.Enabled = true;

         //   //4.例：修改触发器         
         //   ITaskDefinition definition = task.Definition;
         //   //清除
         //   definition.Triggers.Clear();
         //   //添加
         //   ITrigger trigger = definition.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_DAILY);
         //   trigger.Id = "DailyTrigger";
         //   trigger.StartBoundary = "2008-01-01T12:00:00";
         //   trigger.EndBoundary = "2008-01-31T12:00:00";
         //   //更新触发器
         //   folder.RegisterTaskDefinition("TaskA", definition, (int)_TASK_CREATION.TASK_UPDATE,
         //       "",//user
         //       "",//password
         //       _TASK_LOGON_TYPE.TASK_LOGON_NONE, "");
            
         //   //5.例：添加计划任务
         //   ITaskDefinition newTask = scheduler.NewTask(0);
         //   newTask.RegistrationInfo.Author = "Author";
         //   newTask.RegistrationInfo.Description = "My New Task";
         //   newTask.Settings.RunOnlyIfIdle = true;

         //   IDailyTrigger trigger1 = (IDailyTrigger)newTask.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_DAILY);
         //   trigger1.Id = "DailyTrigger";
         //   trigger1.StartBoundary = "2014-01-01T12:00:00";
         //   trigger1.EndBoundary = "2014-01-31T12:00:00";

         //   IEmailAction action = (IEmailAction)newTask.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_SEND_EMAIL);
         //   action.Id = "Email action";
         //   action.Server = "server...";
         //   action.From = "sender...";
         //   action.To = "recipient...";
         //   action.Subject = "The subject of the email...";
         //   action.Body = "The body text of the email...";

         //   IRegisteredTask regTask = folder.RegisterTaskDefinition(
         //        "newTask",
         //        newTask,
         //        (int)_TASK_CREATION.TASK_CREATE_OR_UPDATE,
         //        "", //用户名
         //        "", //密码
         //        _TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN,
         //        "");

         //   IRunningTask runTask = regTask.Run(null);

        #endregion
    }
}
