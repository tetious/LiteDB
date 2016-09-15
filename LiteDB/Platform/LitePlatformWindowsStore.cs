﻿using System;
using System.Threading.Tasks;
#if WINDOWS_UWP
using System.Collections.Specialized;
#endif
using Windows.Storage;
using LiteDB.Platform;

namespace LiteDB.Platform
{
    public class LitePlatformWindowsStore : ILitePlatform
    {
        private readonly LazyLoad<IFileHandler> _fileHandler;
        private readonly LazyLoad<IReflectionHandler> _reflectionHandler;
        
        public LitePlatformWindowsStore() : this (Windows.Storage.ApplicationData.Current.LocalFolder) { }

        public LitePlatformWindowsStore(StorageFolder folder)
        {
#if WINDOWS_UWP
            _fileHandler = new LazyLoad<IFileHandler>(() => new FileHandlerUWP(folder));
#else
            _fileHandler = new LazyLoad<IFileHandler>(() => new FileHandlerWindowsStore(Windows.Storage.ApplicationData.Current.LocalFolder));
#endif
            _reflectionHandler = new LazyLoad<IReflectionHandler>(() => new ExpressionReflectionHandler());

            AddNameCollectionToMapper();
        }

        public IFileHandler FileHandler { get { return _fileHandler.Value; } }

        public IReflectionHandler ReflectionHandler { get { return _reflectionHandler.Value; } }

        public IEncryption GetEncryption(string password)
        {
            return new UWPEncryption(password);
        }

        public void WaitFor(int milliseconds)
        {
            AsyncHelpers.RunSync(() => Task.Delay(milliseconds));
        }

        public void AddNameCollectionToMapper()
        {

        }
    }
}
