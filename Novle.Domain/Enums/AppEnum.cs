using System.Reflection;

namespace Novle.Domain.Enums;

public class StringValueAttribute(string value) : Attribute
{
    public string StringValue { get; protected set; } = value;
}

public static class EnumExtensions
{
    public static string ToValue(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        
        if (fieldInfo?.GetCustomAttributes(typeof(StringValueAttribute), false) is not StringValueAttribute[] attributes) 
            return string.Empty;
        
        return attributes.Length > 0 ? attributes[0].StringValue : string.Empty;
    }
    
    public static T ToEnum<T>(this string value) where T : Enum
    {
        var type = typeof(T);
        if (!type.IsEnum) throw new InvalidOperationException();
        
        foreach (var field in type.GetFields())
        {
            if (field.GetCustomAttributes(typeof(StringValueAttribute), false) is not StringValueAttribute[] attributes) 
                continue;
            
            if (attributes.Length > 0 && attributes[0].StringValue == value)
                return (T)field.GetValue(null)!;
        }
        
        throw new KeyNotFoundException();
    }
}

public enum AppRole
{
    [StringValue("admin")]Admin = 1,
    [StringValue("editor")]Editor = 2,
    [StringValue("user")]User = 3
}

public enum BookStatus
{ 
    [StringValue("ongoing")]OnGoing = 1,
    [StringValue("completed")]Completed = 2,
    [StringValue("paused")]Paused = 3,
    [StringValue("abandoned")]Abandoned = 4,
    [StringValue("draft")]Draft = 5,
}

public enum CommentableEntityType
{
    [StringValue("book")]Book = 1,
    [StringValue("chapter")]Chapter = 2,
    [StringValue("review")]Review = 3,
    [StringValue("comment")]Comment = 4,
}

public enum ReactionType
{
    [StringValue("like")]Like = 1,
    [StringValue("dislike")]Dislike = 2,
    [StringValue("love")]Love = 3,
    [StringValue("laugh")]Laugh = 4,
    [StringValue("sad")]Sad = 5,
    [StringValue("angry")]Angry = 6,
}