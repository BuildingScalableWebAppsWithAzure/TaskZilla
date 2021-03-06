﻿namespace TaskZilla.Models
{
    public enum OpResult
    {
        NoOp, Success, Exception
    }

    /// <summary>
    /// Parent class for our data transport objects. 
    /// </summary>
    public class BaseDTO
    {
        public OpResult Result = OpResult.NoOp;
        public string ErrorMessage; 
    }
}