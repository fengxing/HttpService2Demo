using ApiTranRequest.Hospital;
using ApiTranRequest.Hospital.AddHospital;
using ApiTranRequest.Hospital.GetHospital;
using ApiTranRequest.Hospital.SearchHospital;
using ApiTranService;
using System;
using System.Web.Http;

namespace ApiTranApiDemo.Controllers
{
    /// <summary>
    /// 医院
    /// </summary>
    public class HospitalController : ApiController
    {
        /// <summary>
        /// 添加医院
        /// </summary>
        /// <param name="request"></param>
        [Route("Hospital/AddHospital")]
        [HttpPost]
        [DataAdd]
        [AllowAnonymous]
        public AddHospitalResponse AddHospital(AddHospitalRequest request)
        {
            return HospitalService.AddHospital(request);
        }

        /// <summary>
        /// 修改医院
        /// </summary>
        /// <param name="request"></param>
        /// <exception>医院不存在</exception>
        [Route("Hospital/UpdateHospital")]
        [HttpPost]
        [DataUpdate]
        [AllowAnonymous]
        public void UpdateHospital(UpdateHospitalRequest request)
        {
            HospitalService.UpdateHospital(request);
        }

        /// <summary>
        /// 删除医院
        /// </summary>
        /// <param name="request"></param>
        [Route("Hospital/DeleteHospital")]
        [HttpPost]
        [DataDelete]
        [AllowAnonymous]
        public void DeleteHospital(DeleteHospitalRequest request)
        {
            HospitalService.DeleteHospital(request);
        }

        /// <summary>
        /// 获取医院
        /// </summary>
        /// <param name="request"></param>
        /// <exception>医院不存在</exception>
        [Route("Hospital/GetHospital")]
        [HttpPost]
        [DataQuery]
        [AllowAnonymous]
        public GetHospitalResponse GetHospital(GetHospitalRequest request)
        {
            return HospitalService.GetHospital(request);
        }

        /// <summary>
        /// 检索医院信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("Hospital/SearchHospital")]
        [HttpPost]
        [AllowAnonymous]
        public SearchHospitalResponse SearchHospital(SearchHospitalRequest request)
        {
            return HospitalService.SearchHospital(request);
        }

    }
}
