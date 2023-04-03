# Contact

To get involved or ask questions, join the GDFG Discord server:

- [Join Discord](https://discord.com/invite/2C8eTsU)
- Post your intro in [Contributor Intros](https://github.com/LPGameDevs/EditarrrPublic/discussions/19)
- Read the [Game Design Document](https://github.com/LPGameDevs/EditarrrPublic/wiki/Game-Design-Document)

It is helpful if you let us know what you want to work on by either assigning yourself a project task or by posting a message in discord.

## Video Guide to Editarrr Contributing

[![Video Guide to Editarrr Contributing](https://img.youtube.com/vi/GOCWatlXC2U/0.jpg)](https://www.youtube.com/watch?v=GOCWatlXC2U)




## How to pull down the game to open in Unity

We assume that you have Unity and Git installed as well as have registered an account on github.com.

1. Copy the repo link from github.com

![Remote repo](https://user-images.githubusercontent.com/1744957/229309266-34024879-1ec1-4457-9d97-d6e0a6fcbbf7.png)

``

2. Open a GUI git program or in Terminal pick the location that you store your games. Then clone the repo:

`git clone git@github.com:LPGameDevs/EditarrrPublic.git`

![git console](https://user-images.githubusercontent.com/1744957/229309421-5fef27ba-74a3-437e-bc7b-7f6473ae3978.png)


3. Open the game in Unity, make some changes and save.

4. Use the following commands to commit ("save" your changes in git):

- `git checkout -b feature/my-changes` *Checkout a feature branch for your changes*
- `git add .` *Add all changes*
- `git commit -m "My awesome and descriptive summary of what I have done"` *Give the changes a meaningful short description*

![git commit in terminal](https://user-images.githubusercontent.com/1744957/229309574-dcb56a0f-f54c-4673-8798-541d5dbefb58.png)

5. Push your changes back up to github:

`git push --set-upstream origin feature/my-changes`

6. Go back to github.com and notice there is a message prompting you to create a PR from the new branch. Do it!

![image](https://user-images.githubusercontent.com/1744957/229309746-e65d1e9d-3b88-4eb5-91de-735943761652.png)

7. Give a brief description on the PR of what changes you have made and link to any existing issues that are relevant. Someone will come and review your work and merge it or give you feedback on how it can be improved.


**Good Job! You did it** 😄
