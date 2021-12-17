using builder_mgmt_server.Auth;
using builder_mgmt_server.Controllers.Location;
using builder_mgmt_server.Controllers.User;
using builder_mgmt_server.Database;
using builder_mgmt_server.DOs;
using builder_mgmt_server.Entities;
using builder_mgmt_server.Mappings;
using builder_mgmt_server.Models;
using builder_mgmt_server.Models.Account;
using builder_mgmt_server.Models.Tasks;
using builder_mgmt_server.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace builder_mgmt_server.Controllers
{
    [ApiController]
    [Route("api/location")]
    public class LocationController : BaseApiController
    {

        public LocationController(IAccountModel aModel, IDbOperations db) : base(db)
        {

        }


        [HttpGet]
        [AuthorizeApi]
        public ApiResult SearchLocation(string str)
        {
            if (str == null || str == string.Empty)
            {
                return ResponseHelper.EmptyArray();
            }

            var at = "pk.eyJ1Ijoidm1pa2Vza2EiLCJhIjoiY2t3cW5hcmlkMDhsMTJvbzk2d2xwZGd4ZSJ9.RRD6jE9PYCXpWKEKbhnUpw";
            var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{str}.json?language=cs&access_token={at}";
            //var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{str}.json?proximity=-73.990593%2C40.740121&language=cs&access_token={at}";
            var request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;

            try
            {
                WebResponse response = request.GetResponse();

                var reader = new StreamReader(response.GetResponseStream());
                string jsonStr = reader.ReadToEnd();

                var respObj = JsonConvert.DeserializeObject<MapboxLocationResponse>(jsonStr);

                var res = respObj.features.Select((i) =>
                {
                    var lr = new LocationResponse()
                    {
                        coordinates = i.geometry.coordinates,
                        text1 = i.text_cs,
                        text2 = i.place_name_cs
                    };

                    return lr;
                });

                return ResponseHelper.Successful(res);
            }
            catch
            {
                return ResponseHelper.Successful(new List<LocationResponse>());
            }
        }
    }

    public class LocationResponse
    {
        public List<double> coordinates { get; set; }
        public string text1 { get; set; }
        public string text2 { get; set; }
    }
}
