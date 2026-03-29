using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkKnights.Group3 {
  internal class Rudnev : IKnight{

    public int hilPoints;
    public string name;
    public string specialization {  get; set; }
    public int amountOfMana;
    public int damage;
    public int age;

    public Rudnev() {

      hilPoints = 70;
      name = "Кллиан - древнее зло";
      specialization = "Архимаг";
      amountOfMana = 150;
      damage = 40;
      age = 1863;
    }

    public string GetJobApplication() {

      string background;

      background = "Здравствуй путник, это ты оставлял объявление о поиске мага?. \n" +
                   $"Я {specialization} и владыка некогда разрушенной магической башни и имя мне {name}. " +
                   $"Что? Я слишком молодо выгляжу для такого заявления? Спасибо, однако мне уже {age} года. Это особенность владыки магической башни. " +
                   "После смерти душа перерождается и постепенно вспоминает кем она была, да и живем мы довольно долго. " +
                   "Это из-за того что когда-то очень давно первый владыка побывал у мирового древа и даже попробовал его плоды" +
                   "От того и продолжительность жизни с молодостью и огромный запас маны" +
                   "Однако деньги мне тоже нужны вот и подрабатываю, помогая таким путникам как ты." +
                   "Если тебя всё устраивает, то я готов приступить к выполнению заказа прямо сейчас.";
      return background;

    }

  }
}
