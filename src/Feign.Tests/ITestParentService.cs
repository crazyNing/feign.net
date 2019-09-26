﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Feign.Tests
{
    [CustomFeignClient("yun-platform-service-provider"
    , Fallback = typeof(TestServiceFallback)
    //, FallbackFactory = typeof(TestServiceFallbackFactory)
    //, Url = "http://localhost:8802/"
    //, Url = "http://10.1.5.90:8802/"
    //, Url = "http://localhost:62088/"
    //, Url = ""
    )]
    [NonFeignClient]
    public interface ITestParentService
    {
        [RequestMapping("/{id}", Method = "GET")]
        [MethodId("GetById")]
        Task<string> GetById(object id);
    }
}
