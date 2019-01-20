﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

using ClientProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FaceAuth.Model
{
    public sealed class PersistedFace
    {
        private readonly dynamic _persistedFace = new ExpandoObject();

        public Guid PersistedFaceId { get; }

        public PersistedFace(string addFaceToPersonResponse)
        {
            dynamic response = JsonConvert.DeserializeObject(addFaceToPersonResponse);

            _persistedFace.PersistedFaceId = response.PersistedFaceId;
        }
    }
}
