﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.IO.Pipelines.File
{
    public static class ReadableFilePipelineFactoryExtensions
    {
        public static IPipeReader ReadFile(this PipeFactory factory, string path)
        {
            var pipe = factory.Create();
            var file = new FileReader(pipe.Writer);
            file.OpenReadFile(path);
            return pipe.Reader;
        }
    }
}
