/* ----------------------------------------------------------------------------
 * This file was automatically generated by SWIG (http://www.swig.org).
 * Version 3.0.0
 *
 * Do not make changes to this file unless you know what you are doing--modify
 * the SWIG interface file instead.
 * ----------------------------------------------------------------------------- */

namespace SharpLang.RuntimeInline {

using SharpLLVM;

public class Runtime {
  public static ValueRef define_allocObject(ModuleRef mod) {
      ValueRef ret = new ValueRef(RuntimePINVOKE.define_allocObject(mod.Value));
      return ret;
    }

}

}