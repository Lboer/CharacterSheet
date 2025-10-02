using Microsoft.Maui.ApplicationModel;

namespace DnD_Character_Sheet.Services;

public class PermissionService
{
    public async Task<bool> EnsureStoragePermissionAsync()
    {
#if ANDROID
    var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
    if (status != PermissionStatus.Granted)
    {
        status = await Permissions.RequestAsync<Permissions.StorageWrite>();
    }

    return status == PermissionStatus.Granted;
#else
        return true;
#endif
    }
}
