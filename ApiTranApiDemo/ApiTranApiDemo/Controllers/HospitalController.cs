using ApiTranRequest.Hospital;
using ApiTranRequest.Hospital.Response;
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
    }
}
