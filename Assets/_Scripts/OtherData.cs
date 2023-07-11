using System;

[Flags]
public enum NavmeshMask
{
    basic = 1,
    notWalkable = 2,
    jump = 4,
    inner = 8
}
public static class OtherData
{
    public static Node.Status Invert(this Node.Status status)
    {
        return status == Node.Status.running ? Node.Status.running : status == Node.Status.sucess ? Node.Status.failure : Node.Status.sucess;
    }
}
