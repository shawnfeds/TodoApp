using Grpc.Core;
using Grpc.Net.Client;
using ToDoServer;

namespace ToDoClient
{
    internal class Program
    {
        private static List<Todo> todos = new List<Todo>();

        static async Task Main()
        {
            var serverAddress = " https://localhost:7032";

            var channel = GrpcChannel.ForAddress(serverAddress);

            var client = new Greeter.GreeterClient(channel);

            var keepExecuting = true;
            while (keepExecuting)
            {
                Console.WriteLine("Choose the Todo operation to perform:");
                Console.WriteLine("1.Enter new item");
                Console.WriteLine("2.Read item by ID");
                Console.WriteLine("3.Update item");
                Console.WriteLine("4.Delete item");
                Console.WriteLine("5.Read all items");
                Console.WriteLine("6.Exit");

                Console.WriteLine("Enter the process ID:");
                int processId = Convert.ToInt32(Console.ReadLine());

                switch (processId)
                {
                    case 1:
                        await CreateNewTodoItemAsync(client);
                        break;
                    case 2:
                        await ReadTodoItemById(client);
                        break;
                    case 3:
                        await UpdateTodoItem(client);
                        break;
                    case 4:
                        await DeletedToItemById(client);
                        break;
                    case 5:
                        ReadAllItems();
                        break;
                    case 6:
                        keepExecuting = false;
                        break;
                }
            }
        }

        private static void ReadAllItems()
        {
            if (todos.Count == 0)
            {
                Console.WriteLine("No todo items found in list");
                return;
            }

            foreach (var todo in todos)
            {
                WriteCustomMessage($"Read Todo: ID: {todo.Id} Description: {todo.Description}");
            }
        }

        private static async Task CreateNewTodoItemAsync(Greeter.GreeterClient client)
        {
            Console.WriteLine("Enter item description:");
            var description = Console.ReadLine();

            var newtodo = await client.CreateTodoAsync(new Todo
            {
                Description = description
            });

            todos.Add(newtodo);

            WriteCustomMessage($"Todo created with ID: {newtodo.Id}");
        }

        private static async Task ReadTodoItemById(Greeter.GreeterClient client)
        {
            Console.WriteLine("Enter item id to be read:");
            int id = Convert.ToInt32(Console.ReadLine());

            var readtodo = await client.ReadTodoByIDAsync(new TodoId { Id = id });

            WriteCustomMessage($"Read Todo: ID: {readtodo.Id} Description: {readtodo.Description}");
        }

        private static async Task UpdateTodoItem(Greeter.GreeterClient client)
        {
            Console.WriteLine("Enter item id to be updated");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter new item description:");
            var description = Console.ReadLine();

            var todo = todos.FirstOrDefault(x => x.Id == id);

            if (todo != null)
            {
                todo.Description = description;
            }

            var updateTodo = await client.UpdateTodoAsync(todo);

            WriteCustomMessage($"update todo: Id={updateTodo.Id}");
        }

        private static async Task DeletedToItemById(Greeter.GreeterClient client)
        {
            Console.WriteLine("Enter item id to be deleted");
            int id = Convert.ToInt32(Console.ReadLine());

            await client.DeleteTodoAsync(new TodoId
            {
                Id = id
            });

            var todo = todos.FirstOrDefault(x => x.Id == id);
            todos.Remove(todo);

            WriteCustomMessage("Todo deleted");
        }

        private static void WriteCustomMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}