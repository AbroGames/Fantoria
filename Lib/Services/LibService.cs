using Fantoria.Lib.Services.CmdArgs;
using Fantoria.Lib.Services.Exceptions;
using Fantoria.Lib.Services.Logging;
using Fantoria.Lib.Services.Maths;
using Fantoria.Lib.Services.NotNullChecking;
using Fantoria.Lib.Services.String;

namespace Fantoria.Lib.Services;

public static class LibService
{ 
    public static ExceptionHandlerService ExceptionHandler => ServiceLocator.Get<ExceptionHandlerService>();
    public static CmdArgsService CmdArgs => ServiceLocator.Get<CmdArgsService>();
    public static LogService Log => ServiceLocator.Get<LogService>();
    public static NotNullCheckerService NotNullChecker => ServiceLocator.Get<NotNullCheckerService>();
    public static RandomService Rand => ServiceLocator.Get<RandomService>();
    public static StringCompressService StringCompress => ServiceLocator.Get<StringCompressService>();
    
    public static class Global
    {
        public static LogService Log => LibService.Log; 
        public static NotNullCheckerService NotNullChecker => LibService.NotNullChecker;
    }
}