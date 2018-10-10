using ApiTranDb;
using ApiTranRequest.Hospital;
using ApiTranRequest.Hospital.AddHospital;
using SmartOnlineEntity;
using System;
using ApiTranRequest.Hospital.GetHospital;
using ApiTranRequest.Hospital.SearchHospital;
using SmartHelper.System;
using System.Collections.Generic;
using System.Linq;

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
                HospitalItem = new ApiTranRequest.Hospital.GetHospital.HospitalItem()
                {
                    HospitalID = hospital.ID,
                    Name = hospital.Name
                }
            };

        }

        public static SearchHospitalResponse SearchHospital(SearchHospitalRequest request)
        {
            #region 查询条件
            var whereList = new List<string>();
            if (request.KeyWords.IsNotNull())
            {
                whereList.Add("Name like '%'+@Key+'%'");
            }
            if (request.StartTime.HasValue ||
                request.EndTime.HasValue)
            {
                var time = new TimeFromTo()
                {
                    StartTime = request.StartTime,
                    EndTime = request.EndTime
                };
                var sql = time.ToSql();
                whereList.Add(sql);
            }
            #endregion
            #region 查询
            var pageT = HospitalRep.Search<Hospital>(
              request.PageIndex,
              request.PageSize,
              request.Sort,
              request.GetOrderByField(),
              whereList,
              new
              {
                  Key = request.KeyWords,
              });
            #endregion
            #region 返回
            return new SearchHospitalResponse()
            {
                Count = pageT.Count,
                PageSize = request.PageSize,
                SearchHospitalItems = pageT.Ts.Select(r => new ApiTranRequest.Hospital.SearchHospital.SearchHospitalItem()
                {
                    HospitalItem = new ApiTranRequest.Hospital.SearchHospital.HospitalItem()
                    {
                        HospitalID = r.ID,
                        Name = r.Name,
                    },
                    CreateUserItem = new ApiTranRequest.CreateUserItem()
                    {
                        CreateTime = r.CreateTime,
                        CreateUserID = Guid.Empty,
                        Name = ""
                    }
                })
            };
            #endregion
        }
    }
}
