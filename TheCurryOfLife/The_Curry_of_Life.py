# sway ly
# using the Game class to organize code
# makes it easier to restart game

# --Imports--
import pygame
from math import pi
import random
from timeit import default_timer

# --Global Constants--
BLACK = (0, 0 ,0)
WHITE = (255, 255, 255)
GREEN = (45,122, 9)
RED = (255, 0, 0)
BLUE = (0, 0, 255)
Pie = pi
SCREEN_WIDTH = 500
SCREEN_HEIGHT = 500


# --Classes--

# Collision sprite class
class Curry(pygame.sprite.Sprite):

    def __init__(self):
        # call constructor of parent class
        super().__init__()

        # load collision image
        self.image = pygame.image.load("curry.png").convert_alpha()

        # find rectangle with dimensions that are closest to image
        self.rect = self.image.get_rect()

    # method to reset position of collision sprite if it goes off the window
    def reset_pos(self):
        self.rect.x = random.randrange(460, 750)
        self.rect.y = random.randrange(0, 450)

    # method to move sprite/velocity of sprite
    def update(self):
        self.rect.x -= 1


class Player(pygame.sprite.Sprite):

    def __init__(self):
        super().__init__()

        # load player sprite
        self.image = pygame.image.load("leaf.png").convert_alpha()

        # find rectangle with closest dimensions to put image 'in'
        self.rect = self.image.get_rect()

    # method to move sprite/velocity
    def update(self):
        # get mouse position
        pos = pygame.mouse.get_pos()

        # set position of player sprite to whatever location the mouse is
        self.rect.x = pos[0]
        self.rect.y = pos[1]

# Bullet sprite class
class Bullet(pygame.sprite.Sprite):

    def __init__(self):
        # call Sprite constructor
        super().__init__()

        # load bullet sprite image
        self.image = pygame.image.load("bullet_chopsticks.png").convert_alpha()

        # get closest rectangle shape
        self.rect = self.image.get_rect()

    # primary movement of sprite
    def update(self):
        self.rect.x += 3

""" This class represents an instance/trial of the game. If we need to
           reset the game we'd just need to create a new instance of this
           class. This is seen below in the main() function."""
class Game():


    def __init__(self):

        # This is where we put all our variables that would usually be Global variables
        # but are not necessarily constants
        self.score = 0
        self.game_over = False # this checks if the game has been completed
        self.timer = 7  # countdown
        self.timed = 0
        self.game_lost = False

        # Create sprite lists
        self.curry_list = pygame.sprite.Group()
        self.bullet_list = pygame.sprite.Group()
        self.all_sprites_list = pygame.sprite.Group()


        # Create the curry sprites
        # NOTE: curry sprites represent bowls of spicy curry
        for curry in range(60):
            # create an instance of the Curry class (class of collision sprites)
            self.curry = Curry()

            # create location of sprite/ try defining this in the Curry class
            self.curry.rect.x = random.randrange(100, SCREEN_WIDTH + 20)
            self.curry.rect.y = random.randrange(100, SCREEN_HEIGHT - 50)


            # add curry sprite to both lists
            self.curry_list.add(self.curry)
            self.all_sprites_list.add(self.curry)

        # Creating player sprite
        self.player = Player()
        # add to list of all sprites to allow for collision detection between two sprite lists
        self.all_sprites_list.add(self.player)

        # creating bullet sprite
        # self.bullet = Bullet()

        # background_image
        self.background_image = pygame.image.load("hiddensymbol.png").convert_alpha()

    # processes of events
    def process_events(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                # user has clicked the 'x'
                # return True to signify to game event loop that we want to close the window
                return True
            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_SPACE:
                    # create bullet and move/fire bullet by calling the update function
                    # creating bullet sprite
                    self.bullet = Bullet()


                    # set start position of bullet to wherever the player is
                    self.bullet.rect.x = self.player.rect.x
                    self.bullet.rect.y = self.player.rect.y

                    # add bullet to bullet list and all sprite list to allow for collision detection
                    self.bullet_list.add(self.bullet)
                    self.all_sprites_list.add(self.bullet)



            # when prompted click the screen to restart
            # this part of the function will be called
            # 3 mains steps
            # if user click down on the mouse (1)
            if event.type == pygame.MOUSEBUTTONDOWN:

                # while game is over/player has collected all sprites (2)
                if self.game_over:
                    # then re run the game event constructor and restart the game (3)
                    self.__init__()

        # otherwise, if the user clicks mouse,
        # don't close window because the user is still playing the game
        return False

    # logic for moving the sprites
    def logic(self):

        # if game is not over
        if not self.game_over:

            # move all sprites
            # update automatically assigns the player sprite to the update in the Player class
            # and does the same for the collision sprites in the curry class
            self.all_sprites_list.update()



            # self.bullet accounts for every bullet that has been added to the bullet list
            # using any other iterator such as 'bullet' would only account for one bullet at a time
            # PROBABLY the most recent bullet
            # therefore, only the most recent bullet would be affected by the remainder of the for loop
            for self.bullet in self.bullet_list:
                # See if the player block has collided with anything.
                # True parameter removes sprite from screen
                collision_list = pygame.sprite.spritecollide(self.bullet, self.curry_list, True)

                # Check the list of collisions.
                # if bullet hits block, remove it from its lists
                for curry in collision_list:
                    self.bullet_list.remove(self.bullet)
                    self.all_sprites_list.remove(self.bullet)
                    self.score += 1

                    print(self.score)

                # remove bullet if it flies off the screen
                if self.bullet.rect.x > 450:
                    self.bullet_list.remove(self.bullet)
                    self.all_sprites_list.remove(self.bullet)

                # if there are no more collision sprites in curry_list
                if len(self.curry_list) == 0:
                    # game is flagged as over
                    self.game_over = True

            for self.curry in self.curry_list:

                if self.curry.rect.x <= 0:
                    self.game_over = True
                    self.game_lost = True


    def countdown(self, screen):

        # keeps current time value displayed while self.timer is incrementing
        # this ensures that a time value is seen always on the screen
        font = pygame.font.SysFont("Carlito-BoldItalic", 59)
        text3 = font.render(str(self.timer), True, RED)
        screen.blit(text3, [20, 20])

        # increment time by 1 each time a frame is drawn
        self.timed += 1
        # since frames are drawn at 60 fps,
        # if self.timed equals 60, that means that one second has gone by
        if self.timed == 60:

            # since 1 second has passed, decrease time value displayed on screen
            self.timer -= 1 # countdown

            # display converted integer value to string text and display it
            # reset timer to 0
            font = pygame.font.SysFont("Carlito-BoldItalic", 69) # heheh
            text3 = font.render(str(self.timer), True, RED)
            screen.blit(text3, [20, 20])
            self.timed = 0

        # check if time is up!
        # if the time is up, game is over and they have lost..

        if self.timer <= 0:
            self.game_lost = True
            self.game_over = True

    # display everything to screen
    def display_everything(self, screen):

        # display background image
        screen.blit(self.background_image, [-100, 0])

        # if user has finished game, prompt them to click anywhere again to play again
        # if player has hit all the curry sprites, without letting a curry sprite
        # move off the screen, and there is still more time on the timer
        if self.game_over and self.game_lost == False:
            # make make visible again so they click the x easier
            pygame.mouse.set_visible(True)
            # font = pygame.font.Font("Serif", 25)
            font = pygame.font.SysFont("Carlito-BoldItalic", 40)
            text = font.render("You Win!! Click to restart.", True, GREEN)
            text1 = font.render("THE POWER OF YOUTH", True, GREEN)
            # center_x = (SCREEN_WIDTH // 2) - (text.get_width() // 2)
            # center_y = (SCREEN_HEIGHT // 2) - (text.get_height() // 2)
            screen.blit(text, [50, 50])
            screen.blit(text1, [50, 100])

        if self.game_over and self.game_lost == True:
            # make make visible again so they click the x easier
            pygame.mouse.set_visible(True)
            # font = pygame.font.Font("Serif", 25)
            font = pygame.font.SysFont("Carlito-BoldItalic", 30)
            text = font.render("You've Lost!! Click to retry.", True, RED)
            text1 = font.render("BETTER THAN WE WERE YESTERDAY", True, RED)
            # center_x = (SCREEN_WIDTH // 2) - (text.get_width() // 2)
            # center_y = (SCREEN_HEIGHT // 2) - (text.get_height() // 2)
            screen.blit(text, [50, 50])
            screen.blit(text1, [10, 100])




        # if game is not over, draw everything to screen
        if not self.game_over:
            self.all_sprites_list.draw(screen)
            # display timer
            self.countdown(screen)





        # output everything to screen
        pygame.display.update()



# Main function that runs all the classes when/where needed
def main():


    pygame.init()
    FPS = pygame.time.Clock()

    # window
    SCREEN = [SCREEN_WIDTH, SCREEN_HEIGHT]
    screen = pygame.display.set_mode(SCREEN)
    pygame.display.set_caption("THE CURRY OF LIFE REMASTERED")
    pygame.mouse.set_visible(False)

    done = False

    # create an instance/trial of the Game class
    game = Game()

    while not done:
        # events processing
        # if def process_events returns True, loop/game will end
        # window will clsoe
        done = game.process_events()

        # game logic
        game.logic()

        # draw everything
        game.display_everything(screen)




    # ambitious 60 frames per second
    FPS.tick(60)

    # allows program to end
    # closes window
    pygame.quit()

# only run function if it is being called to run and not just defined
if __name__ == "__main__":
    main()




