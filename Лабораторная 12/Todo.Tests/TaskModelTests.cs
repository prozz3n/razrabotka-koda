using Todo.Entities;
using Xunit;

namespace Todo.Tests
{
    public class TaskModelTests
    {
        [Fact]
        public void NewTask_HasInProgressStatus()
        {
            var task = new TaskModel();

            Assert.False(task.IsDone);
            Assert.Equal("В работе", task.Status);
        }

        [Fact]
        public void IsDone_ChangedToTrue_UpdatesStatus()
        {
            var task = new TaskModel();

            task.IsDone = true;

            Assert.Equal("Выполнено", task.Status);
        }

        [Fact]
        public void IsDone_Changed_RaisesPropertyChangedForStatus()
        {
            var task = new TaskModel();
            string? changed = null;
            task.PropertyChanged += (_, e) => changed = e.PropertyName;

            task.IsDone = true;

            Assert.Equal(nameof(TaskModel.Status), changed);
        }

        [Fact]
        public void NewTask_GeneratesUniqueId()
        {
            var first = new TaskModel();
            var second = new TaskModel();

            Assert.NotEqual(first.Id, second.Id);
        }
    }
}
