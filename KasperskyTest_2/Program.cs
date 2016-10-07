using System;
using System.Collections.Generic;
using System.Linq;

/* Кацак Н.В.
 * Задание №2.
 * Есть коллекция чисел и отдельное число Х.
 * Надо вывести все пары чисел, которые в сумме равны заданному Х.
 */
namespace KasperskyTest_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int sum = 20;
            int[] values = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            List<int> testList = values.ToList<int>();
            getPairs(testList, sum);
            Console.ReadLine();
        }

        public static void getPairs (List<int> _array, int _sum)
        {
            _array.Sort(); // Сортируем список для бинарного поиска
            while (_array.Count > 1) // Если остался один элемент или пусто, выходим
            {
                bool found = false; 
                int first = _array[0];  //первый элемент
                int second = _sum - first; //второй элемент
                int index = _array.BinarySearch(second); //ищем индекс второго элемента
                if (index > 0) // -1 - элемент не найден, 0 - тот же элемент, все остальные подходят
                {
                    Console.WriteLine("{0} {1}", first, second); //выводим
                    _array.RemoveAt(index); // удаляем второй элемент
                    _array.RemoveAt(0); // удаляем первый элемент
                    found = true; 
                }
                if (!found)
                    _array.RemoveAt(0); // если второго не существует, удаляем первый
            }
        }
    }
}
