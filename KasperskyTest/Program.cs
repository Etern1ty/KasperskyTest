using System;
using System.Collections.Generic;
using System.Threading;

/* Кацак Н.В.
 * Задание №1.
 * Надо сделать очередь с операциями push(T) и T pop(). 
 * Операции должны поддерживать обращение с разных потоков. Операция push всегда вставляет и выходит. 
 * Операция pop ждет пока не появится новый элемент. 
 * В качестве контейнера внутри можно использовать только стандартную очередь (Queue) . 
 */
namespace KasperskyTest
{

    class Program
    {
        ProducerConsumer<string> q = new ProducerConsumer<string>();

        static void Main(string[] args)
        {
            new Program().Run();
        }
        

        void Run()
        {
            var threads = new[] { new Thread(Consumer), new Thread(Consumer) }; 
            foreach (var t in threads)
                t.Start();

            string s;
            while ((s = Console.ReadLine()) != "stop") // если в консоль введено 'stop', останавливаем процесс
                q.push(s);

            q.Stop();

            foreach (var t in threads)
                t.Join();
        }

        void Consumer()
        {
            while (true)
            {
                string s = q.pop();
                if (s == null)
                    break;
                Console.WriteLine("Processing: {0}", s);
                Thread.Sleep(2000);
                Console.WriteLine("Processed: {0}", s);
            }
        }
    }

    public class ProducerConsumer<T> where T : class
    {
        object mutex = new object();
        Queue<T> queue = new Queue<T>();
        bool isDead = false;

        public void push(T task) // отправка сообщения в очередь
        {
            if (task == null)
                throw new ArgumentNullException("task");
            lock (mutex)
            {
                if (isDead)
                    throw new InvalidOperationException("Queue already stopped");
                queue.Enqueue(task);
                Monitor.Pulse(mutex);
            }
        }

        public T pop() // вывод сообщения из очереди
        {
            lock (mutex)
            {
                while (queue.Count == 0 && !isDead)
                    Monitor.Wait(mutex);

                if (queue.Count == 0)
                    return null;

                return queue.Dequeue();
            }
        }

        public void Stop()
        {
            lock (mutex)
            {
                isDead = true;
                Monitor.PulseAll(mutex);
            }
        }
    }
}
