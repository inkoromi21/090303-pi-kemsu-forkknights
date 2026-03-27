using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkKnights.Group3 {
  internal class Rudnev : IKnight{

    string name;
    string specialization;
    int amountOfMana;
    int Damage;
    int age;

    public Rudnev() {
      name = "Кллиан - древнее зло";
      specialization = "Архимаг - ";
    }

    public string GetJobApplication() {

      return "test 1.0";

    }

  }
}
