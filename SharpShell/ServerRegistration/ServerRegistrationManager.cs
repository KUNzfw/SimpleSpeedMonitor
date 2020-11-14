using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using Microsoft.Win32;
using SharpShell.Extensions;
using SharpShell.Registry;


namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// THe Server Registration Manager is an object that can be used to
    /// help with Server Registration tasks, such as registering, unregistering
    /// and checking servers. It will work with SharpShell Server objects or
    /// other servers.
    /// </summary>
    public static class ServerRegistrationManager
    {
        /// <summary>
        /// Sets the display name of the a COM server.
        /// </summary>
        /// <param name="classId">The class identifier.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="registrationType">Type of the registration.</param>
        /// <exception cref="InvalidOperationException">Thrown if the class key does not exist for the COM server.</exception>
        public static void SetServerDisplayName(Guid classId, string displayName, RegistrationType registrationType)
        {
            //  Open the class key for the server.
            using (var classesKey = OpenClassesKey(registrationType, RegistryKeyPermissionCheck.ReadWriteSubTree))
            using (var serverKey = classesKey.OpenSubKey(classId.ToRegistryString(), RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.SetValue))
            {
                if (serverKey == null)
                    throw new InvalidOperationException($"Cannot open class id key for '{classId}'");

                //  Set the display name.
                serverKey.SetValue(null, displayName, RegistryValueKind.String);
            }
        }

        /// <summary>
        /// Opens the classes key.
        /// </summary>
        /// <param name="registrationType">Type of the registration.</param>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        private static IRegistryKey OpenClassesKey(RegistrationType registrationType, RegistryKeyPermissionCheck permissions)
        {
            //  Get the classes base key.
            var classesBaseKey = OpenClassesRoot(registrationType);

            //  Open classes.
            var classesKey = classesBaseKey.OpenSubKey(KeyName_Classes, permissions, RegistryRights.QueryValues | RegistryRights.ReadPermissions | RegistryRights.EnumerateSubKeys);
            if (classesKey == null)
                throw new InvalidOperationException("Cannot open classes.");

            return classesKey;
        }

        /// <summary>
        /// Opens the classes root.
        /// </summary>
        /// <param name="registrationType">Type of the registration.</param>
        /// <returns>The classes root key.</returns>
        private static IRegistryKey OpenClassesRoot(RegistrationType registrationType)
        {
            //  Get the registry.
            var registry = ServiceRegistry.ServiceRegistry.GetService<IRegistry>();

            //  Get the classes base key.
            var classesBaseKey = registrationType == RegistrationType.OS64Bit
                ? registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64) :
                registry.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry32);

            //  Return the classes key.
            return classesBaseKey;
        }

        /// <summary>
        /// The classes key name.
        /// </summary>
        private const string KeyName_Classes = @"CLSID";
    }
}
