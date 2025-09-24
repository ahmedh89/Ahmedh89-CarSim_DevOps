using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.DTO;
using DataLogicLibrary.Infrastructure.Enums;
using DataLogicLibrary.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataLogicLibrary.Services.SimulationLogicService;

namespace CarSimulatorTest.Direction
{
    [TestClass]
    public class DirectionTests
    {
        private SimulationLogicService _sut;
        private DirectionContext directionContext;
        private DirectionStrategyResolver directionStrategyResolver;


        public DirectionTests()
        {
            directionContext = new DirectionContext();
            directionStrategyResolver = movementAction =>
            {
                switch (movementAction)
                {
                    case MovementAction.Left:
                        return new TurnLeftStrategy();
                    case MovementAction.Right:
                        return new TurnRightStrategy();
                    case MovementAction.Forward:
                        return new DriveForwardStrategy();
                    case MovementAction.Backward:
                        return new ReverseStrategy();
                    default:
                        throw new KeyNotFoundException();
                }
            };
            _sut = new SimulationLogicService(directionContext, directionStrategyResolver);

        }

        [TestMethod]
        public void Forward_Action_CardinalDirection_Remains_North_When_Previous_Movement_Is_Not_Backward()
        {

            var status = new StatusDTO()
            {
                CardinalDirection = CardinalDirection.North,
                GasValue = 20,
                EnergyValue = 20,
                MovementAction = MovementAction.Left
            };
            var userInputForward = 3;

            var expectedDirection = CardinalDirection.North;

            var result = _sut.PerformAction(userInputForward, status);

            Assert.AreEqual(expectedDirection, result.CardinalDirection);

        }

        [TestMethod]
        public void Forward_Action_CardinalDirection_Remains_West_When_Previous_Movement_Is_Not_Backward()
        {
            // Arrange
            var status = new StatusDTO()
            {
                CardinalDirection = CardinalDirection.West,
                GasValue = 20,
                EnergyValue = 20,
                MovementAction = MovementAction.Left
            };
            var userInputForward = 3;

            var expectedDirection = CardinalDirection.West;

            var result = _sut.PerformAction(userInputForward, status);

            Assert.AreEqual(expectedDirection, result.CardinalDirection);

        }

        [TestMethod]

        public void Forward_Action_CardinalDirection_Changes_To_South_When_Previous_Movement_Is_Backward()
        {
            var status = new StatusDTO()
            {
                CardinalDirection = CardinalDirection.North,
                GasValue = 20,
                EnergyValue = 20,
                MovementAction = MovementAction.Backward
            };
            var userInputForward = 3;
            var expectedDirection = CardinalDirection.South;

            var result = _sut.PerformAction(userInputForward, status);

            Assert.AreEqual(expectedDirection, result.CardinalDirection);
        }

        [TestMethod]
        public void Backward_Action_From_North_Sets_South_When_Previous_Not_Backward()
        {

            var status = new StatusDTO
            {
                CardinalDirection = CardinalDirection.North,
                GasValue = 20,
                EnergyValue = 20,
                MovementAction = MovementAction.Forward
            };
            var userInputBackward = 4;
            var expectedDirection = CardinalDirection.South;


            var result = _sut.PerformAction(userInputBackward, status);


            Assert.AreEqual(expectedDirection, result.CardinalDirection);
            Assert.AreEqual(MovementAction.Backward, result.MovementAction);
        }

        [TestMethod]

        public void Forward_Action_CardinalDirection_Remains_East_When_Previous_Movement_Is_Not_Backward()
        {
            var status = new StatusDTO()
            {
                CardinalDirection = CardinalDirection.East,
                GasValue = 20,
                EnergyValue = 20,
                MovementAction = MovementAction.Right,
            };
            var userInputForward = 3;
            var expectedDirection = CardinalDirection.East;

            var result = _sut.PerformAction(userInputForward, status);

            Assert.AreEqual(expectedDirection, result.CardinalDirection);
            Assert.AreEqual(MovementAction.Forward, result.MovementAction);

        }