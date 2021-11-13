# Cross Platform Encrypted File System

The idea is to create a cross platform file system in .NET.

The underlying system for Linux would be `Fuse` and for Windows this would `dokany` with their fuse compatible layer.

## Step 1

This is still a work in progress but the general idea in the first proof of concept is as followed:

- A base layer that is a translation between the OS and .NET
- A layer that implements the actions as much as possible in pure .NET
- A layer that implements the OS specific actions that were not possible in the previous layer

Once this is done then nothing more than the `Redirect` sample of the original `Fuse` in .NET is done, but the groundwork should be there to run it cross platform.
  
## Step 2

The result of step 1 is a cross platform that just mounts a directory and passes all commands.  
In this second step the idea is to encrypt all the filenames and file content using the built-in .NET `AES` functionality.

[Here is a blog post](https://ourcodeworld.com/articles/read/471/how-to-encrypt-and-decrypt-files-using-the-aes-encryption-algorithm-in-c-sharp) that can help getting started.

## Step 3

In step 2 there is already an `encrypted filesystem`, but the files and directories are still on disk somewhere.  

In this step, as preparation for step 4, a complete tree index should be stored in an SQLite database using the `Closure Tables` principle. This can later be replaced with methods if I do some benchmarks.

[See this `stackoverlfow` post for some more idea's](https://stackoverflow.com/questions/4048151/what-are-the-options-for-storing-hierarchical-data-in-a-relational-database)

## Step 4

When all of the above steps work it's time to try and put the file data inside a container, being one big encrypted file.  
In a first version the encrypted file will be addressed in blocks, each block has a number and and represents a `4kb` data block.
Files are assigned blocks that do not need to be sequential.

## Step 5

This step is to solve the issue of the ever growing file that is used in step 4, this can be done by re-using blocks that were previously freed (deleted data).

## Step 6

The biggest issue with the result of step 5 is fragmentation, it is much easier and faster to read sequential data inside the file.

In this step a background process should run to optimize the existing file structure and move data blocks around so that file data is stored sequential.

## Step 7

Even with all of the above done, the `FUSE` version used is still a very very old 2.6 one, this step is to upgrade the entire interface to a newer one.  
This might have to happen sooner as the `dokany` fuse compatible interface might be for a different version.