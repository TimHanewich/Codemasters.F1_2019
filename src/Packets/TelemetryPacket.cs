using System;
using System.Collections.Generic;
using TimHanewichToolkit;

namespace Codemasters.F1_2019
{
    public class TelemetryPacket : Packet
    {
        public CarTelemetryData[] FieldTelemetryData { get; set; }

        public override void LoadBytes(byte[] bytes)
        {
            ByteArrayManager BAM = new ByteArrayManager(bytes);
            base.LoadBytes(BAM.NextBytes(23)); //Load header

            int t = 0;
            List<CarTelemetryData> TelDatas = new List<CarTelemetryData>();
            for (t = 1; t <= 20; t++)
            {
                TelDatas.Add(CarTelemetryData.Create(BAM.NextBytes(66)));
            }
            FieldTelemetryData = TelDatas.ToArray();

            //I skipped the field "ButtonStatus".  Doesn't seem needed.

        }

        public class CarTelemetryData
        {
            public ushort SpeedKph { get; set; }
            public ushort SpeedMph { get; set; }
            public float Throttle { get; set; }
            public float Steer { get; set; }
            public float Brake { get; set; }
            public float Clutch { get; set; }
            public sbyte Gear { get; set; }
            public ushort EngineRpm { get; set; }
            public bool DrsActive { get; set; }
            public byte RevLightsPercentage { get; set; }
            public WheelDataArray BrakeTemperature { get; set; }
            public WheelDataArray TyreSurfaceTemperature { get; set; }
            public WheelDataArray TyreInnerTemperature { get; set; }
            public ushort EngineTemperature { get; set; }
            public WheelDataArray TyrePressure { get; set; }
            public SurfaceType SurfaceType_RearLeft { get; set; }
            public SurfaceType SurfaceType_RearRight { get; set; }
            public SurfaceType SurfaceType_FrontLeft { get; set; }
            public SurfaceType SurfaceType_FrontRight { get; set; }

            public static CarTelemetryData Create(byte[] bytes)
            {
                CarTelemetryData ReturnInstance = new CarTelemetryData();

                ByteArrayManager BAM = new ByteArrayManager(bytes);

                //Get speed
                ReturnInstance.SpeedKph = BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                double MPH = ReturnInstance.SpeedKph * 0.621371;
                ReturnInstance.SpeedMph = (ushort)MPH;

                //Get throttle
                ReturnInstance.Throttle = BitConverter.ToSingle(BAM.NextBytes(4), 0);

                //Get steer
                ReturnInstance.Steer = BitConverter.ToSingle(BAM.NextBytes(4), 0);

                //Get brake
                ReturnInstance.Brake = BitConverter.ToSingle(BAM.NextBytes(4), 0);

                //Get clutch
                ReturnInstance.Clutch = BAM.NextByte();

                //Get gear
                ReturnInstance.Gear = (sbyte)BAM.NextByte();

                //Get engine RMP
                ReturnInstance.EngineRpm = BitConverter.ToUInt16(BAM.NextBytes(2), 0);

                //Drs on or not
                byte nb = BAM.NextByte();
                if (nb == 0)
                {
                    ReturnInstance.DrsActive = false;
                }
                else if (nb == 1)
                {
                    ReturnInstance.DrsActive = true;
                }

                //Get engine rev lights percentage
                ReturnInstance.RevLightsPercentage = BAM.NextByte();

                //get brake temperature
                ReturnInstance.BrakeTemperature = new WheelDataArray();
                ReturnInstance.BrakeTemperature.RearLeft = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.BrakeTemperature.RearRight = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.BrakeTemperature.FrontLeft = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.BrakeTemperature.FrontRight = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);

                //get tyre surface temperature
                ReturnInstance.TyreSurfaceTemperature = new WheelDataArray();
                ReturnInstance.TyreSurfaceTemperature.RearLeft = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.TyreSurfaceTemperature.RearRight = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.TyreSurfaceTemperature.FrontLeft = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.TyreSurfaceTemperature.FrontRight = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);

                //get tyre Inner Temperature
                ReturnInstance.TyreInnerTemperature = new WheelDataArray();
                ReturnInstance.TyreInnerTemperature.RearLeft = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.TyreInnerTemperature.RearRight = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.TyreInnerTemperature.FrontLeft = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);
                ReturnInstance.TyreInnerTemperature.FrontRight = (float)BitConverter.ToUInt16(BAM.NextBytes(2), 0);

                //Get engine temperature
                ReturnInstance.EngineTemperature = BitConverter.ToUInt16(BAM.NextBytes(2), 0);

                //Get tyre pressure
                ReturnInstance.TyrePressure = new WheelDataArray();
                ReturnInstance.TyrePressure.RearLeft = BitConverter.ToSingle(BAM.NextBytes(4), 0);
                ReturnInstance.TyrePressure.RearRight = BitConverter.ToSingle(BAM.NextBytes(4), 0);
                ReturnInstance.TyrePressure.FrontLeft = BitConverter.ToSingle(BAM.NextBytes(4), 0);
                ReturnInstance.TyrePressure.FrontRight = BitConverter.ToSingle(BAM.NextBytes(4), 0);

                //Get surface type
                ReturnInstance.SurfaceType_RearLeft = CodemastersToolkit.GetSurfaceTypeFromSurfaceTypeId(BAM.NextByte(), Game.F1_2019);
                ReturnInstance.SurfaceType_RearRight = CodemastersToolkit.GetSurfaceTypeFromSurfaceTypeId(BAM.NextByte(), Game.F1_2019);
                ReturnInstance.SurfaceType_FrontLeft = CodemastersToolkit.GetSurfaceTypeFromSurfaceTypeId(BAM.NextByte(), Game.F1_2019);
                ReturnInstance.SurfaceType_FrontRight = CodemastersToolkit.GetSurfaceTypeFromSurfaceTypeId(BAM.NextByte(), Game.F1_2019);

                return ReturnInstance;
            }

        }
    }

}