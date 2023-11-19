using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Id3Lib
{
    /// <summary>
    /// Declare the type of tags a frame will manage
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class Id3FrameTagAttribute:Attribute
    {
        string _tag;

        /// <summary>
        /// Default Constuctor
        /// </summary>
        /// <param name="tag">The tag pattern to match</param>
        public Id3FrameTagAttribute(string tag)
        {
            _tag = tag;
        }

        /// <summary>
        /// Get the tag pattern to match
        /// </summary>
        public string Tag
        {
            get { return _tag; }
        }
    }
}
