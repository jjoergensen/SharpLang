// THIS FILE AUTOMATICALLY GENERATED BY xpidl2cs.pl
// EDITING IS PROBABLY UNWISE
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2007, 2008 Novell, Inc.
//
// Authors:
//	Andreia Gaita (avidigal@novell.com)
//

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;

namespace Mono.Mozilla {

	[Guid ("0d0acd2a-61b4-11d4-9877-00c04fa0cf4a")]
	[InterfaceType (ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport ()]
	internal interface nsIOutputStream {

#region nsIOutputStream
		[PreserveSigAttribute]
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int close ();

		[PreserveSigAttribute]
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int flush ();

		[PreserveSigAttribute]
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int write ([MarshalAs (UnmanagedType.LPStr) ]  string aBuf,
				 uint aCount,
				out uint ret);

		[PreserveSigAttribute]
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int writeFrom ([MarshalAs (UnmanagedType.Interface) ]  nsIInputStream aFromStream,
				 uint aCount,
				out uint ret);

		[PreserveSigAttribute]
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int writeSegments ( nsIReadSegmentFunDelegate aReader,
				 IntPtr aClosure,
				 uint aCount,
				out uint ret);

		[PreserveSigAttribute]
		[MethodImpl (MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int isNonBlocking (out bool ret);

#endregion
	}


	internal class nsOutputStream {
		public static nsIOutputStream GetProxy (Mono.WebBrowser.IWebBrowser control, nsIOutputStream obj)
		{
			object o = Base.GetProxyForObject (control, typeof(nsIOutputStream).GUID, obj);
			return o as nsIOutputStream;
		}
	}
}
#if example

using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;

	internal class OutputStream : nsIOutputStream {

#region nsIOutputStream
		int nsIOutputStream.close ()
		{
			return ;
		}



		int nsIOutputStream.flush ()
		{
			return ;
		}



		int nsIOutputStream.write ([MarshalAs (UnmanagedType.LPStr) ]  string aBuf,
				 uint aCount,
				out uint ret)
		{
			return ;
		}



		int nsIOutputStream.writeFrom ([MarshalAs (UnmanagedType.Interface) ]  nsIInputStream aFromStream,
				 uint aCount,
				out uint ret)
		{
			return ;
		}



		int nsIOutputStream.writeSegments ( nsIReadSegmentFunDelegate aReader,
				 IntPtr aClosure,
				 uint aCount,
				out uint ret)
		{
			return ;
		}



		int nsIOutputStream.isNonBlocking (out bool ret)
		{
			return ;
		}



#endregion
	}
#endif