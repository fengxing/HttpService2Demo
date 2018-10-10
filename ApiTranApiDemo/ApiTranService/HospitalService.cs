using ApiTranDb;
using ApiTranRequest.Hospital;
using ApiTranRequest.Hospital.AddHospital;
using SmartOnlineEntity;
using System;
using ApiTranRequest.Hospital.GetHospital;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <exception>医院不存在</exception>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception>医院不存在</exception>
        public static GetHospitalResponse GetHospital(GetHospitalRequest request)
        {
            var hospital = HospitalRep.GetByID(request.HospitalID).ThrowIfNull("医院不存在");
            return new GetHospitalResponse()
            {
                CreateUserItem = new ApiTranRequest.CreateUserItem()
                {
                    CreateTime = hospital.CreateTime,
                    CreateUserID = Guid.Empty,
                    Name = ""
                },
                UpdateUserItem = new ApiTranRequest.UpdateUserItem()
                {
                    UpdateUserID = Guid.Empty,
                    Name = "",
                    UpdateTime = hospital.UpdateTime,
                },
                HospitalItem = new HospitalItem()
                {
                    HospitalID = hospital.ID,
                    Name = hospital.Name
                }
            };

        }
    }
}
