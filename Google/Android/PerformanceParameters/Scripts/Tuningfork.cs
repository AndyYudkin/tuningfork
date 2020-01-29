﻿//-----------------------------------------------------------------------
// <copyright file="Tuningfork.cs" company="Google">
//
// Copyright 2020 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Google.Android.PerformanceParameters
{
    public partial class Tuningfork
    {
        /// <summary>
        ///     Action called every time new fidelity parameters are set.
        /// </summary>
        public Action<FidelityParams> OnReceiveFidelityParameters;

        /// <summary>
        ///     Action called every time log is uploaded.
        /// </summary>
        public Action<UploadTelemetryRequest> OnReceiveUploadLog;

        /// <summary>
        ///     Start tuningfork.
        /// </summary>
        /// <returns> Code returned by tuningfork library. </returns>
        public TFErrorCode Start()
        {
            return StartInternal();
        }

        /// <summary>
        ///     Stop tuningfork.
        /// </summary>
        /// <returns>Code returned by tuningfork library.</returns>
        public TFErrorCode Stop()
        {
            if (m_OnStop != null) m_OnStop();
            m_OnStop = null;
            return m_Library.Destroy();
        }

        /// <summary>
        ///     A blocking call to get fidelity parameters from the server.
        ///     You do not need to call this if you pass in a fidelity_params_callback as part of the settings to TuningFork_init.
        ///     Note that once fidelity parameters are downloaded, any timing information is recorded as being associated with
        ///     those parameters.
        ///     If you subsequently call GetFidelityParameters and a new set of parameters is downloaded, any data that is already
        ///     collected will be submitted to the backend.
        ///     The parameter request is sent to:
        ///     url_base + 'applications/' + package_name + '/apks/' + version_number + ':generateTuningParameters'
        /// </summary>
        /// <param name="defaultFidelity">these will be assumed current if no parameters could be downloaded</param>
        /// <param name="initialTimeoutMs">time to wait before returning from this call when no connection can be made</param>
        /// <returns>
        ///     TFERROR_TIMEOUT if there was a timeout before params could be downloaded.
        ///     TFERROR_OK if there was a timeout before params could be downloaded.
        /// </returns>
        public Result<FidelityParams> GetFidelityParameters(FidelityParams defaultFidelity, uint initialTimeoutMs)
        {
            return m_AdditionalLibraryMethods.GetFidelityParameters(defaultFidelity, initialTimeoutMs);
        }

        /// <summary>
        ///     Set the current annotation.
        /// </summary>
        /// <param name="annotation">current annotation.</param>
        /// <returns>
        ///     TFERROR_INVALID_ANNOTATION if annotation is inconsistent with the settings.
        ///     TFERROR_OK on success.
        /// </returns>
        public TFErrorCode SetCurrentAnnotation(Annotation annotation)
        {
            return m_AdditionalLibraryMethods.SetCurrentAnnotation(annotation);
        }

        /// <summary>
        ///     Record a frame tick that will be associated with the instrumentation key and the current annotation.
        ///     For both advanced and default mode FrameTick is called automatically.
        /// </summary>
        /// <param name="key">An instrument key.</param>
        /// <returns>
        ///     TFERROR_INVALID_INSTRUMENT_KEY if the instrument key is invalid.
        ///     TFERROR_OK on success.
        /// </returns>
        public TFErrorCode FrameTick(InstrumentationKeys key)
        {
            return m_Library.FrameTick(key);
        }

        /// <summary>
        ///     Force upload of the current histograms.
        ///     * Return TFERROR_OK if the upload could be initiated.
        ///     * Return TFERROR_PREVIOUS_UPLOAD_PENDING if there is a previous upload blocking this one.
        ///     * Return TFERROR_UPLOAD_TOO_FREQUENT if less than a minute has elapsed since the previous upload.
        /// </summary>
        /// <returns>Status of flush operation.</returns>
        public TFErrorCode Flush()
        {
            return m_Library.Flush();
        }

        /// <summary>
        ///     Load fidelity parameters from the APK "assets/tuningfork/" folder.
        /// </summary>
        /// <param name="filename">name of the file</param>
        /// <returns>The fidelity parameters, if successfully loaded.</returns>
        public Result<FidelityParams> FindFidelityParametersInApk(string filename)
        {
            return m_AdditionalLibraryMethods.FindFidelityParametersInApk(filename);
        }

        /// <summary>
        ///     Return if swappy is enabled or not.
        ///     To enable swappy in Editor go to:
        ///     <b>Project Settings ->  Player -> Resolution and Presentation</b> and activate <b>Optimized Frame Pacing</b>.
        ///     It is recommended to turn on swappy to archive better frame rate.
        /// </summary>
        /// <returns>True is swappy is enabled</returns>
        public bool SwappyIsEnabled()
        {
            return m_Library.SwappyIsEnabled();
        }

        /// <summary>
        ///     Set the currently active fidelity parameters.
        ///     This function overrides any parameters that have been downloaded if in experiment mode.
        ///     Use this when, for instance, the player has manually changed the game quality settings.
        ///     This flushes (i.e. uploads) any data associated with any previous parameters.
        /// </summary>
        /// <param name="fidelityParams">The new fidelity parameters</param>
        /// <returns>TFERROR_OK if the parameters could be set.</returns>
        public TFErrorCode SetFidelityParameters(FidelityParams fidelityParams)
        {
            return m_AdditionalLibraryMethods.SetFidelityParameters(fidelityParams);
        }
    }
}
