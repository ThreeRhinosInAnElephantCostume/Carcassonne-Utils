#if GODOT
using System.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;
using ExtraMath;
using Godot;
using Newtonsoft.Json;
using static System.Math;
using Expression = System.Linq.Expressions.Expression;

public static partial class Utils
{
    public delegate void SignalHandler(params object[] args);
    public class SignalHandlerObject : Godot.Object
    {
        public SignalHandler handler;
        public void Grabber()
        {
            handler();
        }
        public void Grabber(object arg0)
        {
            handler(arg0);
        }
        public void Grabber(object arg0, object arg1)
        {
            handler(arg0, arg1);
        }
        public void Grabber(object arg0, object arg1, object arg2)
        {
            handler(arg0, arg1, arg2);
        }
        public void Grabber(object arg0, object arg1, object arg2, object arg3)
        {
            handler(arg0, arg1, arg2, arg3);
        }
        public void Grabber(object arg0, object arg1, object arg2, object arg3, object arg4)
        {
            handler(arg0, arg1, arg2, arg3, arg4);
        }
        public void Grabber(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5)
        {
            handler(arg0, arg1, arg2, arg3, arg4, arg5);
        }
        public void Grabber(Godot.Collections.Array args)
        {
            object[] oargs = new object[args.Count];
            for(int i = 0; i < args.Count; i++)
            {
                oargs[i] = args[i];
            }
            handler(oargs);
        }
        public void Grabber(string arg0)
        {
            handler(arg0);
        }
        public void Grabber(float arg0)
        {
            handler(arg0);
        }
        public void Grabber(bool arg0)
        {
            handler(arg0);
        }
        public void Grabber(int arg0)
        {
            handler(arg0);
        }
        public void Grabber(long arg0)
        {
            handler(arg0);
        }
        public void Grabber(Vector2 arg0)
        {
            handler(arg0);
        }
        public void Grabber(Vector3 arg0)
        {
            handler(arg0);
        }
        public void Grabber(InputEvent arg0)
        {
            handler(arg0);
        }
        public void Grabber(Camera arg0, InputEvent arg1, Vector3 arg2, Vector3 arg3, int arg4)
        {
            handler(arg0, arg1, arg2, arg3, arg4);
        }
        public SignalHandlerObject(SignalHandler handler)
        {
            this.handler = handler;
        }
        
    }
    public class ConnectionHandle
    {
        protected Godot.Object _obj;
        protected string _signal;
        protected SignalHandlerObject _handler;
        public Godot.Object SignalEmitter {get => _obj;}
        public bool Connected {get; protected set;} = true;
        public void Disconnect(bool ignoreerrors = false)
        {
            if(!ignoreerrors)
                Assert(Connected, $"Attempted to disconnect the signal {_signal} of object {_obj.GetObjectTypeAndName()} multiple times" );
            _obj.Disconnect(_signal, _handler, ignoreerrors, ignoreerrors);
            Connected = false;
        }
        public ConnectionHandle(Godot.Object obj, string signal, SignalHandlerObject handler)
        {
            this._obj = obj;
            this._signal = signal;
            this._obj = handler;
        }
    }
    public static ConnectionHandle Connect(this Godot.Object obj, string signal, SignalHandler func, bool autodisconnect = true, bool ignoreerrors = false)
    {
        if(!ignoreerrors)
            Assert(obj.HasSignal(signal));
        if(autodisconnect)
            obj.Disconnect(signal, func, true, true);
        var sho = new SignalHandlerObject(func);
        Error err = obj.Connect(signal, sho, nameof(sho.Grabber));
        if(!ignoreerrors)
            Assert(err, $"Failed to connect signal {signal} of object {obj.GetObjectTypeAndName()}");
        return new ConnectionHandle(obj, signal, sho);
    }
    public static void Disconnect(this Godot.Object obj, string signal, SignalHandlerObject handler, bool ignorenotconnected = true, bool ignoreerrors = false)
    {
        if(!ignoreerrors)
            Assert(obj.HasSignal(signal), $"object {obj.GetObjectTypeAndName()} does not name a signal {signal}");
        var conns = obj.GetSignalConnectionList(signal);
        foreach(Godot.Collections.Dictionary it in conns)
        {
            if(it["target"] is SignalHandlerObject sho)
            {
                if(sho == handler)
                {
                    obj.Disconnect(signal, sho, nameof(sho.Grabber));
                    return;
                }
            }
        }
        if(!ignorenotconnected)
            Assert(false, "Did not find the specified connection for " + signal);
    }
    public static void Disconnect(this Godot.Object obj, string signal, SignalHandler func, bool ignorenotconnected = true, bool ignoreerrors = false)
    {
        if(!ignoreerrors)
            Assert(obj.HasSignal(signal), $"object {obj.GetObjectTypeAndName()} does not name a signal {signal}");
        var conns = obj.GetSignalConnectionList(signal);
        foreach(Godot.Collections.Dictionary it in conns)
        {
            if(it["target"] is SignalHandlerObject sho)
            {
                if(sho.handler == func || sho.handler.Method == func.Method)
                {
                    obj.Disconnect(signal, sho, nameof(sho.Grabber));
                }
            }
        }
        if(!ignorenotconnected)
            Assert(false, "Did not find the specified connection for " + signal);
    }
    public static void Disconnect(this Godot.Object obj, ConnectionHandle handle, bool ignoreerrors = false)
    {
        if(!ignoreerrors)
            Assert(handle.SignalEmitter == obj, $@"The signal emitter ({handle.SignalEmitter.GetObjectTypeAndName()})
                did not match the current object ({handle.SignalEmitter.GetObjectTypeAndName()})");
        handle.Disconnect(ignoreerrors);
    }
    public static int DisconnectAll(this Godot.Object obj, string signal)
    {
        var conns = obj.GetSignalConnectionList(signal);
        foreach(Godot.Collections.Dictionary it in conns)
        {
            obj.Disconnect((string)it["signal"], (Godot.Object)it["target"], (string)it["method"]);
        }
        return conns.Count;
    }
    static T GetElementOrDefault<T>(T[] a, int i)
    {
        if(a.Length <= i)
            return default(T);
        return a[i];
    }
    static T GetElementOrDefault<T>(object[] a, int i)
    {
        if(a.Length <= i)
            return default(T);
        var v = a[i];
        if(v == null)
            return default(T);
        return (T)v;
    }
    public static ConnectionHandle OnButtonPressed(this BaseButton obj, Action func)
    {
        return Connect(obj, "pressed", (args) => func());
    }
    public static ConnectionHandle OnToggled(this BaseButton obj, Action<bool> func)
    {
        return Connect(obj, "toggled", (args) =>
        {
            Assert(args.Length == 1, "Invalid number of arguments");
            Assert(args[0] is bool, $"Invalid argument type: {args[0]}");
            func((bool)args[0]);
        });
    }
    public static ConnectionHandle OnValueChanged(this Slider obj, Action<float> func)
    {
        return Connect(obj, "value_changed", (args) =>
        {
            Assert(args.Length == 1, "Invalid number of arguments");
            Assert(args[0] is float, $"Invalid argument type: {args[0]}");
            func((float)args[0]);
        });
    }
    public static ConnectionHandle OnMouseEntered(this Area obj, Action func)
    {
        return Connect(obj, "mouse_entered", (args) => func());
    }
    public static ConnectionHandle OnMouseExited(this Area obj, Action func)
    {
        return Connect(obj, "mouse_exited", (args) => func());
    }
    public static ConnectionHandle OnInputEvent(this Area obj, Action<Camera, InputEvent, Vector3, Vector3, int> func)
    {
        return Connect(obj, "input_event", (args) => 
        {
            Assert(args.Length == 5, "Invalid number of arguments");
            func((Camera)args[0], (InputEvent)args[1], (Vector3)args[2], (Vector3)args[3], (int)args[4]);
        });
    }

    public static ConnectionHandle OnFileSelected(this FileDialog dialog, Action<string> func)
    {
        return Connect(dialog, "file_selected", (args) =>
        {
            Assert(args.Length == 1, "Invalid number of arguments");
            Assert(args[0] is string, $"Invalid argument type: {args[0]}");
            func((string)args[0]);
        });
    }
    public static ConnectionHandle OnDirectorySelected(this FileDialog dialog, Action<string> func)
    {
        return Connect(dialog, "dir_selected", (args) =>
        {
            Assert(args.Length == 1, "Invalid number of arguments");
            Assert(args[0] is string, $"Invalid argument type: {args[0]}");
            func((string)args[0]);
        });
    }
    public static ConnectionHandle OnAccepted(this AcceptDialog dialog, Action func)
    {
        return Connect(dialog, "confirmed", (args) => func());
    }
    public static ConnectionHandle OnConfirmed(this AcceptDialog dialog, Action func)
    {
        return Connect(dialog, "confirmed", (args) => func());
    }
    public static ConnectionHandle OnConfirmed(this ConfirmationDialog dialog, Action func)
    {
        return Connect(dialog, "confirmed", (args) => func());
    }
    public static ConnectionHandle OnCancelled(this ConfirmationDialog dialog, Action func)
    {
        return dialog.GetCancel().OnButtonPressed(func);
    }

    public static ConnectionHandle OnItemActivated(this ItemList list, Action<int> func)
    {
        return Connect(list, "item_activated", (args) => func((int)args[0]));
    }
    public static ConnectionHandle OnItemSelected(this ItemList list, Action<int> func)
    {
        return Connect(list, "item_selected", (args) => func((int)args[0]));
    }
    public static ConnectionHandle OnNothingSelected(this ItemList list, Action func)
    {
        return Connect(list, "nothing_selected", (args) => func());
    }

    public static ConnectionHandle OnTextChanged(this LineEdit edit, Action<string> func)
    {
        return Connect(edit, "text_changed", (args) => func((string)args[0]));
    }
    public static ConnectionHandle OnTextSubmitted(this LineEdit edit, Action<string> func)
    {
        return Connect(edit, "text_entered", (args) => func((string)args[0]));
    }

    public static ConnectionHandle OnDraw(this CanvasItem item, Action func)
    {
        return Connect(item, "draw", (args) => func());
    }
    public static ConnectionHandle OnHide(this CanvasItem item, Action func)
    {
        return Connect(item, "hide", (args) => func());
    }
    public static ConnectionHandle OnItemRectChanged(this CanvasItem item, Action func)
    {
        return Connect(item, "item_rect_changed", (args) => func());
    }
    public static ConnectionHandle OnVisibilityChanged(this CanvasItem item, Action<bool> func)
    {
        return Connect(item, "visibility_changed", (args) => func(item.Visible));
    }

    
    public static ConnectionHandle OnFocusEntered(this Control control, Action func)
    {
        return Connect(control, "focus_entered", (args) => func());
    }
    public static ConnectionHandle OnFocusExited(this Control control, Action func)
    {
        return Connect(control, "focus_exited", (args) => func());
    }
    public static ConnectionHandle OnGuiInput(this Control control, Action<InputEvent> func)
    {
        return Connect(control, "gui_input", (args) => func((InputEvent)args[0]));
    }
    public static ConnectionHandle OnMinimumSizeChanged(this Control control, Action func)
    {
        return Connect(control, "minimum_size_changed", (args) => func());
    }
    public static ConnectionHandle OnModalClosed(this Control control, Action func)
    {
        return Connect(control, "modal_closed", (args) => func());
    }
    public static ConnectionHandle OnMouseEntered(this Control control, Action func)
    {
        return Connect(control, "mouse_entered", (args) => func());
    }
    public static ConnectionHandle OnMouseExited(this Control control, Action func)
    {
        return Connect(control, "mouse_exited", (args) => func());
    }
    public static ConnectionHandle OnResized(this Control control, Action func)
    {
        return Connect(control, "resized", (args) => func());
    }
    public static ConnectionHandle OnSizeFlagsChanged(this Control control, Action func)
    {
        return Connect(control, "size_flags_changed", (args) => func());
    }

    public static ConnectionHandle OnReady(this Node node, Action func)
    {
        return Connect(node, "ready", (args) => func());
    }
    public static ConnectionHandle OnRenamed(this Node node, Action func)
    {
        return Connect(node, "renamed", (args) => func());
    }
    public static ConnectionHandle OnTreeEntered(this Node node, Action func)
    {
        return Connect(node, "tree_entered", (args) => func());
    }
    public static ConnectionHandle OnTreeExited(this Node node, Action func)
    {
        return Connect(node, "tree_exited", (args) => func());
    }
    public static ConnectionHandle OnTreeExiting(this Node node, Action func)
    {
        return Connect(node, "tree_exiting", (args) => func());
    }

    public static ConnectionHandle OnGameplayEntered(this Spatial spatial, Action func)
    {
        return Connect(spatial, "gameplay_entered", (args) => func());
    }
    public static ConnectionHandle OnGameplayExited(this Spatial spatial, Action func)
    {
        return Connect(spatial, "gameplay_exited", (args) => func());
    }
    public static ConnectionHandle OnVisibilityChanged(this Spatial spatial, Action<bool> func)
    {
        return Connect(spatial, "gameplay_entered", (args) => func(spatial.Visible));
    }
}

#endif