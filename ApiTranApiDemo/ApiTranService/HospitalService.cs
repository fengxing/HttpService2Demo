﻿using System;
using ApiTranDb;
using ApiTranRequest.Hospital;
using ApiTranRequest.Hospital.Response;
using SmartOnlineEntity;

namespace ApiTranService
{
    public class HospitalService
    {
        public static AddHospitalResponse AddHospital(AddHospitalRequest request)
        {
            var hospital = new Hospital()
            {
                Name = request.Name,
            };
            HospitalRep.Add(hospital);
            return new AddHospitalResponse()
            {
                HospitalID = hospital.ID
            };
        }

        public static void UpdateHospital(UpdateHospitalRequest request)
        {
            var hospital = HospitalRep.GetByID(request.HospitalID).ThrowIfNull("医院不存在");
            hospital.Name = request.Name;
            HospitalRep.Update(hospital);
        }
    }
}
