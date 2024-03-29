﻿namespace ReadersRealm.Common.Exceptions.Services;

public class ServiceInterfaceNotFound : BaseNotFoundException
{
    private const string DefaultMessage = "No interface was found for the provided service.";

	public ServiceInterfaceNotFound() 
        : base(DefaultMessage) { }

    public ServiceInterfaceNotFound(string message)
	    : base(message) { }
}