using ApiTranDb;
using ApiTranRequest.Hospital;
using ApiTranRequest.Hospital.AddHospital;
using SmartOnlineEntity;
using System;

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

        public static void DeleteHospital(DeleteHospitalRequest request)
        {
            var hospital = HospitalRep.GetByID(request.HospitalID);
            if (hospital != null)
            {
                HospitalRep.Delete(hospital);
            }
        }
    }
}
