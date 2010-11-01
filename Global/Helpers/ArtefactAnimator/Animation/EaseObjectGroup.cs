/*
    Copyright © 2009 Jesse Graupmann
    All rights reserved.

    Redistribution and use in source and binary forms, with or without 
    modification, are permitted provided that the following conditions 
    are met:

        * Redistributions of source code must retain the above copyright
          notice, this list of conditions and the following disclaimer.
        * Redistributions in binary form must reproduce the above copyright 
          notice, this list of conditions and the following disclaimer 
          in the documentation and/or other materials provided with the 
          distribution.
        * Neither the name of the author nor the names of contributors 
          may be used to endorse or promote products derived from this 
          software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
    "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
    LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
    FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
    COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
    INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
    BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
    CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
    LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
    ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
    POSSIBILITY OF SUCH DAMAGE.
*/



using System.Collections.Generic;

namespace Artefact.Animation
{
    public delegate void EaseObjectGroupCompleteHandler(EaseObjectGroup easeObjectGroup);

    public class EaseObjectGroup
    {
        public List<IEaseObject> Items = new List<IEaseObject>();
        public List<IEaseObject> RunningItems = new List<IEaseObject>();
        public bool UseComplete = true;
        public bool UseStoppedEvent;

        #region EASE OBJECT METHODS

        /// <summary>
        /// Runs EaseObject.Finish() for each item in RunningItems and triggers the Complete function.
        /// </summary>
        /// <returns>
        /// If the RunningItems count is greater than 0.
        /// </returns>
        public bool FinishGroup()
        {
            IEaseObject[] list = RunningItems.ToArray();
            foreach (IEaseObject eo in list) eo.Finish();
            if (Complete != null) Complete(this);
            return list.Length > 0;
        }

        /// <summary>
        /// Runs EaseObject.Stop() for each item in RunningItems and triggers the Complete function.
        /// </summary>
        /// <returns>
        /// If the RunningItems count is greater than 0.
        /// </returns>
        public bool StopGroup()
        {
            IEaseObject[] list = RunningItems.ToArray();
            foreach (IEaseObject eo in list) eo.Stop();
            return list.Length > 0;
        }

        #endregion

        #region RESET

        /// <summary>
        /// Runs ClearGroup and ClearComplete
        /// </summary>
        public void Reset()
        {
            ClearGroup();
            ClearComplete();
        }

        /// <summary>
        /// Removes all Items and RunningItems
        /// </summary>
        public void ClearGroup()
        {
            Items = new List<IEaseObject>();
            RunningItems = new List<IEaseObject>();
        }

        /// <summary>
        /// Removes any Complete listeners
        /// </summary>
        public void ClearComplete()
        {
            Complete = null;
        }

        #endregion

        #region ADDING

        /// <summary>
        /// Adds EaseObject and subscribes to Complete & Stopped events
        /// </summary>
        /// <param name="eo"></param>
        public void AddEaseObject(IEaseObject eo)
        {
            if ( UseComplete ) eo.Complete += EoComplete;
            if ( UseStoppedEvent ) eo.Stopped += EoComplete; // if stopped by another ease object -> continue

            Items.Add(eo);
            RunningItems.Add(eo);
        }

        #endregion

        #region EVENTS

        /// <summary>
        /// Triggers Complete event if RunningItems.Count is 0
        /// </summary>
        /// <param name="eo"></param>
        private void EoComplete(IEaseObject easeObject, double percent)
        {
            if (!RunningItems.Contains(easeObject)) return;
            RunningItems.Remove(easeObject);
            if (RunningItems.Count > 0) return;
            if (Complete != null) Complete(this);
        }

        #endregion

        public event EaseObjectGroupCompleteHandler Complete;
    }
}