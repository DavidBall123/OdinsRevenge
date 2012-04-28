using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OdinsRevenge
{

    // class looks after the players stats and updates the stat bars. 
    class PlayerResourceController
    {

        private const int MANA_REDUCTION = 20; // the amount of mana that should be reduced each time a spell is cast.
        private const int HEALTH_REDUCTION = 10;
        private const double ENERGY_RECHARGE_RATE = 0.4;
        private const int STARTING_MANA = 100;
        private const int STARTING_ENERGY = 100;
        private const int STARTING_HEALTH = 10;

        // Amount of hit points that player has
        private int health;
        private int mana;
        private double energy;

        public int Health
        {
            get { return health; }
            set { health = value; }
        }


        public int Mana
        {
            get { return mana; }
            set { mana = value; }
        }

        public double Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        public PlayerResourceController()
        {
            Health = STARTING_HEALTH;
            Mana = STARTING_MANA;
            Energy = STARTING_ENERGY;
        }

        public void ReduceMana()
        {
            mana = mana - MANA_REDUCTION; 
        }

        public void ReduceHealth()
        {
            health = health - HEALTH_REDUCTION;
        }

        public void EnergyRecharge()
        {
            energy = energy + ENERGY_RECHARGE_RATE;
        }
    }
}

