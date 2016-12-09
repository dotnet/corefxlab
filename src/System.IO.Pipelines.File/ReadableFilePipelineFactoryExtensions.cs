﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.IO.Pipelines.File
{
    public static class ReadableFilePipelineFactoryExtensions
    {
        public static IPipelineReader ReadFile(this PipelineFactory factory, string path)
        {
            var reader = factory.Create();

            var file = new FileReader(reader);
            file.OpenReadFile(path);
            return file;
        }
    }
}
