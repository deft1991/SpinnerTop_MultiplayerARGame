// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: enums/matchmake_status.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SpinnerKing.Interop {

  /// <summary>Holder for reflection information generated from enums/matchmake_status.proto</summary>
  public static partial class MatchmakeStatusReflection {

    #region Descriptor
    /// <summary>File descriptor for enums/matchmake_status.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static MatchmakeStatusReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChxlbnVtcy9tYXRjaG1ha2Vfc3RhdHVzLnByb3RvEhRjb20ucHJvamVjdHgu",
            "aW50ZXJvcCpkCg9NYXRjaG1ha2VTdGF0dXMSEgoOR0FNRV9OT1RfRk9VTkQQ",
            "ABILCgdXQUlUSU5HEAESDAoIQ09NUExFVEUQAhIKCgZDTE9TRUQQAxIWChJO",
            "T19BVkFJTEFCTEVfU0xPVFMQBEITqgIQUHJvamVjdFguSW50ZXJvcGIGcHJv",
            "dG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::SpinnerKing.Interop.MatchmakeStatus), }, null));
    }
    #endregion

  }
  #region Enums
  public enum MatchmakeStatus {
    [pbr::OriginalName("GAME_NOT_FOUND")] GameNotFound = 0,
    [pbr::OriginalName("WAITING")] Waiting = 1,
    [pbr::OriginalName("COMPLETE")] Complete = 2,
    [pbr::OriginalName("CLOSED")] Closed = 3,
    [pbr::OriginalName("NO_AVAILABLE_SLOTS")] NoAvailableSlots = 4,
  }

  #endregion

}

#endregion Designer generated code