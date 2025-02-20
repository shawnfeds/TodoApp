using Grpc.Core;
using ToDoServer;

namespace ToDoServer.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public static List<Todo> todoList = new List<Todo>();

        public override Task<Todo> CreateTodo(Todo request, ServerCallContext context)
        {
            int newId = todoList.Count + 1;

            request.Id = newId;
            todoList.Add(request);

            return Task.FromResult(request);
        }

        public override Task<Todo> ReadTodoByID(TodoId request, ServerCallContext context)
        {
            var todo = todoList.FirstOrDefault(t => t.Id == request.Id);

            if (todo != null)
            {
                return Task.FromResult(todo);
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Todo not Found"));
            }
        }

        public override Task<Todo> UpdateTodo(Todo request, ServerCallContext context)
        {
            var todo = todoList.FirstOrDefault(t => t.Id == request.Id);

            if (todo != null)
            {
                todo.Description = request.Description;
                return Task.FromResult(todo);
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Todo not Found"));
            }
        }

        public override Task<Google.Protobuf.WellKnownTypes.Empty> DeleteTodo(TodoId request, ServerCallContext context)
        {
            var todo = todoList.FirstOrDefault(t => t.Id == request.Id);

            if (todo != null)
            {
                todoList.Remove(todo);
                return Task.FromResult(new Google.Protobuf.WellKnownTypes.Empty());
            }
            else
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Todo not Found"));
            }
        }
    }
}
