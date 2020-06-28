using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ConsoleApp1
{
    public class Question3
    {
        /// <returns>
        /// returns the maximum waiting time for any car in the queue.
        /// </returns>
        public int Solution(int[] requiredFuelList, int X, int Y, int Z)
        {
            var petrolStation = CreatePetrolStation(X, Y, Z);
            var carList = CreateCarQueue(requiredFuelList);

            while (
                (AreThereCarsInTheQueue(carList) || 
                 AreThereCarsInTheFuelPump(petrolStation)
                ) 
                && PetrolStationCanFuelNextCarInTheQueue(petrolStation, carList)
                )
            {
                PlaceCarsToPumps(petrolStation, carList);
                petrolStation.PumpOneUnitFuel();
                AdvanceTimer(carList);
                petrolStation.RemoveFullCarsFromPumps();

                Debug.WriteLine($"{_timeElapsed} {JsonConvert.SerializeObject(requiredFuelList)} {JsonConvert.SerializeObject(petrolStation)}");
            }
            var maxWaitingTime = carList.Max(car => car.WaitingTime);
            var retval = maxWaitingTime == 0 ? -1 : maxWaitingTime;
            return retval;
        }

        private bool AreThereCarsInTheFuelPump(PetrolStation petrolStation)
        {
            return petrolStation.PumpsArray.Any(a => a.IsBusy);
        }

        private void PlaceCarsToPumps(PetrolStation petrolStation, List<Car> carList)
        {
            var carsInTheQueue = carList.Where(a => a.IsInTheQueue).ToList();
            for (int i = 0; i < carsInTheQueue.Count; i++)
            {
                var oneCar = carsInTheQueue[i];

                if (oneCar == null)
                    return;

                var fuelPump = petrolStation.GetFuelPumpAvailableForPetrolRequired(oneCar.RequiredFuel);
                if (fuelPump != null)
                {
                    oneCar.IsInTheQueue = false;
                    fuelPump.ServingOneCar = oneCar;
                }
                else
                {
                    break;
                }
            }
        }

        private bool PetrolStationCanFuelNextCarInTheQueue(PetrolStation petrolStation, List<Car> carList)
        {
            var carQueue = carList.Where(a => a.IsInTheQueue).ToList();
            if (carQueue.Count == 0)
                return true;

            var oneCar = carList.FirstOrDefault(a => a.IsInTheQueue);

            var canFuelAllCarsWaiting = petrolStation.MaxPumpCapacity() >= oneCar.RequiredFuel;

            return canFuelAllCarsWaiting;
        }

        private bool AreThereCarsInTheQueue(List<Car> carQueue)
        {
            return carQueue.Count(a => a.IsInTheQueue) > 0;
        }


        int _timeElapsed = 0;
        private void AdvanceTimer(List<Car> carList)
        {
            _timeElapsed++;
            carList.ForEach(car =>
            {
                if (car.IsInTheQueue)
                    car.WaitingTime++;
            });
        }

        private List<Car> CreateCarQueue(int[] requiredFuelList)
        {
            var carList = new List<Car>();
            requiredFuelList
                    .ToList()
                    .ForEach(reqFuel => carList.Add(new Car { RequiredFuel = reqFuel, IsInTheQueue = true }));

            return carList;
        }

        private PetrolStation CreatePetrolStation(int x, int y, int z)
        {
            var petrolStation = new PetrolStation(new FuelPump[] {
                new FuelPump{ Capacity=x },
                new FuelPump{ Capacity=y },
                new FuelPump{ Capacity=z },
            });
            return petrolStation;
        }

        class Car
        {
            public int RequiredFuel { get; set; }
            public int WaitingTime { get; set; }
            public bool IsInTheQueue { get; set; }
        }

        class FuelPump
        {
            public Car ServingOneCar { get; set; } = null;
            public int Capacity { get; set; }
            public bool IsBusy { get { return ServingOneCar != null; } }

            public void PumpOneUnitFuel()
            {
                if (ServingOneCar != null && 
                    ServingOneCar.RequiredFuel > 0)
                {
                    ServingOneCar.RequiredFuel--;
                    Capacity--;
                }
            }
        }

        class PetrolStation
        {
            public FuelPump[] PumpsArray;
            
            public PetrolStation(FuelPump [] fuelPumpArray)
            {
                PumpsArray = fuelPumpArray;
            }

            public FuelPump GetFuelPumpAvailableForPetrolRequired(int petrolRequired)
            {
                FuelPump fuelPump = null;
                for (int i = 0; i < PumpsArray.Length; i++)
                {
                    if (petrolRequired <= PumpsArray[i].Capacity &&
                        !PumpsArray[i].IsBusy 
                        )
                    {
                        fuelPump = PumpsArray[i];
                        break;
                    }
                }
                return fuelPump;
            }

            public int MaxPumpCapacity()
            {
                return PumpsArray.Max(a => a.Capacity);
            }

            
            private int FindPumpAvailable(Car car)
            {
                for (int i = 0; i < PumpsArray.Length; i++)
                {
                    var pump = PumpsArray[i];
                    if (!pump.IsBusy && pump.Capacity >= car.RequiredFuel)
                        return i;
                }
                return -1;
            }

            internal void PumpOneUnitFuel()
            {
                foreach (var fuelPump in PumpsArray)
                {
                    fuelPump.PumpOneUnitFuel();
                }
            }

            internal void RemoveFullCarsFromPumps()
            {
                foreach (var pump in PumpsArray)
                {
                    if (pump.ServingOneCar?.RequiredFuel == 0)
                    {
                        pump.ServingOneCar = null;
                    }
                }
            }
        }

    }
}
