using System;
using System.Diagnostics;
using Godot;

namespace Fantoria.Lib.Services.Exceptions;

[Service]
public class ExceptionHandlerService
{
    public void AddExceptionHandlerForUnhandledException()
    {
        AppDomain.CurrentDomain.UnhandledException += HandleException;
    }
    
    private void HandleException(object sender, UnhandledExceptionEventArgs args)
    {
        Log.Info();
        // If logging will produce unhandled exception then we fucked up, so we need try/catch here.
        try
        {
            if (args.ExceptionObject is not Exception exception) return;
			
            Log.Error(exception.ToString());
        }
        catch (Exception e)
        {
            // Use GD.Print instead of Log.Error to avoid infinite recursion.
            GD.Print($"Unexpected exception was thrown while handling unhandled exception: {e}");
        }
		
        Debugger.Break();
    }
}
