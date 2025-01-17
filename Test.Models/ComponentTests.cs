﻿using System;
using System.Collections.Generic;
using System.Linq;
using AstroAdapt.Models;
using Xunit;

namespace Test.Models
{
    public class ComponentTests
    {
        public static IEnumerable<object[]> GetComponents => SampleData.FullPermutationsOfComponents();

        [Fact]
        public void ConstructsWithDefaultModelName()
        {
            // arrange
            var target = new Component();

            // act and assert
            Assert.Equal("Generic", target.Model);
        }

        [Theory]
        [InlineData(true,false,false)]
        [InlineData(false,false,false)]
        [InlineData(false,true,true)]
        public void EqualityWorks(bool isNull, bool isSameId, bool expected)
        {
            Component src = new ();
            Component? tgt = null;
            if (!isNull)
            {
                tgt = new();
                if (isSameId)
                {
                    tgt.Id = src.Id;
                }
            }
            Assert.Equal(expected, src.Equals(tgt));
        }

        [Theory]
        [MemberData(nameof(GetComponents))]
        public void ToStringProvidesExpectedData(Component component)
        {
            // arrange
            // act
            var str = component.ToString();

            // assert
            Assert.Contains(component.Manufacturer.Name, str);
            Assert.Contains(component.Model, str);
            Assert.Contains(component.TargetDirectionConnectionSize.ToString(), str);
            Assert.Contains(component.TargetDirectionConnectionType.AsVisualString(true), str);
            Assert.Contains(component.SensorDirectionConnectionSize.ToString(), str);
            Assert.Contains(component.SensorDirectionConnectionType.AsVisualString(false), str);
            Assert.Contains(component.LengthMm.ToString(), str);
            if (component.BackFocusMm > 0)
            {
                Assert.Contains(component.BackFocusMm.ToString(), str);
            }
            else
            {
                Assert.DoesNotContain("bf=", str);
            }

            if (component.ThreadRecessMm > 0)
            {
                Assert.Contains(component.ThreadRecessMm.ToString(), str);
            }
            else
            {
                Assert.DoesNotContain("thr=", str);
            }
        }

        [Fact]
        public void ReverseDoesNothingWhenReversibleIsFalse()
        {
            // arrange
            var src = new Component
            {
                TargetDirectionConnectionType = ConnectionTypes.Inserter,
                TargetDirectionConnectionSize = ConnectionSizes.M42,
                SensorDirectionConnectionType = ConnectionTypes.Receiver,
                SensorDirectionConnectionSize = ConnectionSizes.M12,               
            };

            // act
            src.Reverse();

            // assert
            Assert.Equal(ConnectionTypes.Inserter, src.TargetDirectionConnectionType);
            Assert.Equal(ConnectionSizes.M42, src.TargetDirectionConnectionSize);
            Assert.Equal(ConnectionTypes.Receiver, src.SensorDirectionConnectionType);
            Assert.Equal(ConnectionSizes.M12, src.SensorDirectionConnectionSize);
        }

        [Fact]
        public void ReverseSwapsTargetAndSensorOrientations()
        {
            // arrange
            var src = new Component
            {
                TargetDirectionConnectionType = ConnectionTypes.Inserter,
                TargetDirectionConnectionSize = ConnectionSizes.M42,
                SensorDirectionConnectionType = ConnectionTypes.Receiver,
                SensorDirectionConnectionSize = ConnectionSizes.M12,
                IsReversible = true,
            };

            // act
            src.Reverse();

            // assert
            Assert.Equal(ConnectionTypes.Inserter, src.SensorDirectionConnectionType);
            Assert.Equal(ConnectionSizes.M42, src.SensorDirectionConnectionSize);
            Assert.Equal(ConnectionTypes.Receiver, src.TargetDirectionConnectionType);
            Assert.Equal(ConnectionSizes.M12, src.TargetDirectionConnectionSize);
        }

        [Fact]
        public void HashCodeIsHashCodeOfId()
        {
            // arrange
            var target = new Component();

            // act
            var hashCode = target.Id.GetHashCode();

            // assert
            Assert.Equal(hashCode, target.GetHashCode());
        }
    }
}
