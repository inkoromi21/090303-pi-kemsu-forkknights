//обратите внимание на блок using

using System;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace ForkKnights {
  class Program {
    // Я буду спрашивать КАЖДУЮ строчку. Разберитесь как тут все работает.
    // Кода может показаться много, но он не очень сложный.
    static void Main() {

      Console.OutputEncoding = System.Text.Encoding.UTF8; //Что изменится если убрать UTF8?

      // Данные о проекте через рефлексию.
      Console.WriteLine($"📦 Проект: {Assembly.GetExecutingAssembly().GetName().Name}");
      Console.WriteLine($"📁 Путь: {Assembly.GetExecutingAssembly().Location}");
      Console.WriteLine();
      
      Console.WriteLine(" 🌟 ДОБРО ПОЖАЛОВАТЬ В МУЛЬТИВСЕЛЕННУЮ ЦИФРОВЫХ ВИТЯЗЕЙ! 🌟");
      Console.WriteLine("=========================================================");
      Console.WriteLine();

      // Получаем все ТИПЫ в текущей сборке через РЕФЛЕКСИЮ. Ныряем глубже.
      // Отличное время, чтобы вспомнить что такое "класс"
      var CustomTypeList = Assembly.GetExecutingAssembly().GetTypes();

      // Создаем List<object>[], ищем в проекте все классы, подходящие под условия с помощью механизма LINQ, 
      // затем создаем экземпляр каждого класса и добавляем этот экземпляр в List. Что такое LINQ?
      // Есть ли что-то подобное в других языках?
      var KnightList = CustomTypeList
          .Where(CustomTypeItem => CustomTypeItem.IsClass && !CustomTypeItem.IsAbstract)
          .Where(CustomTypeItem => CustomTypeItem.Namespace != null && CustomTypeItem.Namespace.Contains("ForkKnights"))
          .Where(CustomTypeItem => CustomTypeItem.GetInterface(nameof(IKnight)) != null) //попробуйте закоментировать эту и 2 строки ниже
          .Where(CustomTypeItem => !CustomTypeItem.Name.Contains("<>")) // что это???
          .Where(CustomTypeItem => CustomTypeItem != typeof(Program))   // а это зачем? а ещё можно GetCustomAttribute наляпать
          .Select(CustomTypeItem => Activator.CreateInstance(CustomTypeItem))
          .ToList();

      if (!KnightList.Any()) {
        Console.WriteLine("⚠️ Выдано ноль лицензий ... Ожидается Pull Request!");
        Console.ReadKey();
        return; //!NB - зачем тут пустой return? Попробуйте изменить код так, чтобы дойти сюда.
      }

      Console.WriteLine($" ⚔  В мультивселенной лицензировано ратных витязей: {KnightList.Count} ⚔ ");
      Console.WriteLine();

      int KnightIndex = 0;
      foreach (var KnightItem in KnightList) {
        Console.WriteLine($"============== ВИТЯЗЬ #{++KnightIndex:00} \"============="); //Что это за :00 ?   
        Console.WriteLine($" Класс: {KnightItem.GetType().Name}");
        Console.WriteLine($" Пространство: {KnightItem.GetType().Namespace}");

        Console.WriteLine($" Свойства:");
        // Получаем все свойства с модификаторами public И instance. К instance относятся НЕстатические поля.
        var PropertyList = KnightItem.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        if (PropertyList.Length == 0) {
          Console.WriteLine("   (нет публичных свойств)");
        } else {
          foreach (var PropertyItem in PropertyList) {
            // Можно и без LINQ. Кстати, зачем эти проверки?
            if (PropertyItem.Name == "GetType" || PropertyItem.Name == "ToString") {
              continue;
            }
            object PropertyValue = PropertyItem.GetValue(KnightItem);
            Console.WriteLine($"   {PropertyItem.Name}: {PropertyValue}");
          }
        }

        Console.WriteLine($" Поля:"); //Да. C# на уровне рефлексии может отличить поля от свойств. Как?        
        var FieldList = KnightItem.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        if (FieldList.Length == 0) {
          Console.WriteLine("   (нет публичных полей)");
        } else {
          //а можно ли заменить на обычный for? 
          foreach (var FieldItem in FieldList) {
            object FieldValue = FieldItem.GetValue(KnightItem);

            // Добавим небольшое усложнение. На случай, если одним из полей будет коллекция.
            // Для свойств мы не сделали этого осознанно. Попробуйте добавить и обратите внимание на разницу.
            // Кстати, что будет, если закоментировать && !(FieldValue is string)? Почему?
            // Что будет, если полем окажется коллекция коллекций в стиле List<List<Mage>>?

            if (FieldValue is IEnumerable Collection && !(FieldValue is string)) { 

              Console.Write($"   {FieldItem.Name}: [ ");

              foreach (var Item in Collection) {
                Console.Write($"{Item} ");
              }

              Console.WriteLine("]");
            } else {
              Console.WriteLine($"   {FieldItem.Name}: {FieldValue}");
            }

          }
        }

        // Мы можем сделать просто Console.WriteLine((KnightItem as IKnight).GetJobApplication());
        // но это не наш метод. Мы будет делать через рефексию.
        Console.WriteLine($" Методы:");
        var MethodList = KnightItem.GetType()
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(MethodItem => MethodItem.ReturnType == typeof(string))  //нам всё-таки нужна строка для Сonsole.WriteLine
            .Where(MethodItem => MethodItem.GetParameters().Length == 0)
            .Where(MethodItem => !MethodItem.IsSpecialName) //а это что такое?
            .Where(MethodItem => MethodItem.DeclaringType != typeof(object));


        //Обратите внимание. .ToList(); в конце отстуствует, но foreach справляется - почему так? Как лучше в каких ситуациях?
        //Что такое IEnumerable и IEnumerator. Чем отличатся объект-запрос от коллекции?


        //KnightItem.GetType().GetMethod("GetJobApplication"); а ещё можно было по имени, но мы не ищем легких путей
        //к тому же мы хотим вызвать вообще все методы!
        foreach (var MethodItem in MethodList) {

          // Вопрос на засыпку. Чем отличаются две строки ниже друг от друга? Как лучше?
          // string result = MethodItem.Invoke(KnightItem, null) as string 
          string result = (string)MethodItem.Invoke(KnightItem, null);  //что за null? за что отвечает второй параметр?

          Console.WriteLine($"   {MethodItem.Name}(): {result}");
        }
        Console.WriteLine();
      }

      Console.WriteLine("========================================");
      Console.WriteLine("🎮 Для выхода - нажать любую клавишу...");
      Console.ReadKey();
    }
  }
}