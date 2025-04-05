<template>
  <div class="home-container">
    <div class="grimoire-wrapper">
      <div class="grimoire-decoration left"></div>

      <div class="grimoire-pages">
        <div class="home-header">
          <h1 class="main-title">Welcome to Your Grimoire</h1>
          <p class="subtitle">Welcome, seeker of mystical wisdom</p>
        </div>

        <div class="content-wrapper">
          <div class="intro-section">
            <div class="seal-container">
              <div class="home-seal"></div>
            </div>
            <div class="intro-text">
              <p class="greeting">Greetings, worthy practitioner of the arcane arts.</p>
              <p>Within these enchanted pages lies a collection of spells gathered from across the realms.
                Browse the ancient wisdom, search for specific incantations, or filter by school of magic.</p>
              <p>May these spells serve you well on your journey.</p>
            </div>
          </div>

          <div class="lore-section">
            <div class="parchment">
              <h2 class="lore-title">From the Archmage's Notes</h2>
              <p class="lore-text">
                "Remember that magic flows through all things, connecting the fabric of reality itself.
                A true wizard understands that spells are not simply tools, but expressions of the
                universe's underlying forces. Study diligently, cast wisely, and always respect the
                balance of powers."
              </p>
              <div class="signature">- Archmage Elminster</div>
            </div>
          </div>
        </div>
      </div>

      <div class="grimoire-decoration right"></div>
    </div>
  </div>
</template>

<style scoped>
.home-container {
  width: 100%;
  font-family: 'Cinzel', serif;
  color: var(--color-text);
  margin: 0 auto;
  padding: 20px 0;
}

.grimoire-wrapper {
  display: flex;
  width: 100%;
  position: relative;
  min-height: 80vh;
  margin: 20px auto;
  padding: 0;
}

.grimoire-decoration {
  width: 50px;
  position: relative;
  background-repeat: no-repeat;
  background-size: cover;
}

.grimoire-decoration.left {
  border-radius: 20px 0 0 20px;
  background: linear-gradient(to right, var(--color-accent), var(--header-bg));
  box-shadow: inset -5px 0 10px rgba(0, 0, 0, 0.3);
  position: relative;
}

.grimoire-decoration.right {
  border-radius: 0 20px 20px 0;
  background: linear-gradient(to left, var(--color-accent), var(--header-bg));
  box-shadow: inset 5px 0 10px rgba(0, 0, 0, 0.3);
}

.grimoire-decoration.left::before,
.grimoire-decoration.right::before {
  content: "";
  position: absolute;
  top: 50%;
  width: 20px;
  height: 20px;
  background-color: var(--color-secondary);
  border-radius: 50%;
  box-shadow: 0 0 10px var(--color-primary);
}

.grimoire-decoration.left::before {
  right: 10px;
  transform: translateY(-50%);
}

.grimoire-decoration.right::before {
  left: 10px;
  transform: translateY(-50%);
}

.grimoire-decoration.left::after,
.grimoire-decoration.right::after {
  content: "";
  position: absolute;
  height: 80%;
  width: 5px;
  top: 10%;
  background: linear-gradient(to bottom, var(--color-border), transparent, var(--color-border));
}

.grimoire-decoration.left::after {
  right: 5px;
}

.grimoire-decoration.right::after {
  left: 5px;
}

.grimoire-pages {
  flex: 1;
  background-color: var(--color-background);
  padding: 30px;
  border-top: 5px double var(--color-border);
  border-bottom: 5px double var(--color-border);
  position: relative;
  box-shadow: inset 0 0 30px rgba(0, 0, 0, 0.2);
  background-image:
    linear-gradient(rgba(255, 255, 255, 0.05) 1px, transparent 1px),
    linear-gradient(90deg, rgba(255, 255, 255, 0.05) 1px, transparent 1px);
  background-size: 20px 20px;
}

.grimoire-pages::before {
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  width: 30px;
  background: linear-gradient(to right, rgba(0, 0, 0, 0.1), transparent);
}

.grimoire-pages::after {
  content: '';
  position: absolute;
  top: 0;
  bottom: 0;
  right: 0;
  width: 30px;
  background: linear-gradient(to left, rgba(0, 0, 0, 0.1), transparent);
}

.home-header {
  text-align: center;
  margin-bottom: 40px;
  position: relative;
}

.home-header::after {
  content: "";
  position: absolute;
  bottom: -15px;
  left: 50%;
  transform: translateX(-50%);
  width: 200px;
  height: 4px;
  background: linear-gradient(90deg, transparent, var(--color-primary), transparent);
  box-shadow: 0 0 10px var(--color-primary);
}

.main-title {
  font-size: 3rem;
  font-weight: 700;
  margin: 0;
  color: var(--color-primary);
  text-shadow: 1px 1px 3px var(--shadow-color);
  letter-spacing: 3px;
}

.subtitle {
  font-size: 1.4rem;
  margin-top: 10px;
  font-style: italic;
  opacity: 0.85;
}

.content-wrapper {
  display: flex;
  flex-direction: column;
  gap: 40px;
}

/* Introduction Section */
.intro-section {
  display: flex;
  align-items: center;
  gap: 30px;
  padding: 20px;
  margin-bottom: 20px;
}

.seal-container {
  flex: 0 0 150px;
}

.home-seal {
  width: 150px;
  height: 150px;
  background-color: rgba(139, 0, 0, 0.1);
  border: 3px solid var(--color-primary);
  border-radius: 50%;
  position: relative;
  box-shadow: 0 0 15px rgba(139, 0, 0, 0.2);
}

.home-seal::before {
  content: "";
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 80%;
  height: 80%;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 100 100' fill='none' stroke='%238b0000' stroke-width='2'%3E%3Ccircle cx='50' cy='50' r='35'/%3E%3Cpath d='M50 15v70M15 50h70M29.29 29.29l41.42 41.42M29.29 70.71l41.42-41.42M50 15l20 35L50 85 30 50z'/%3E%3C/svg%3E");
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
}

.intro-text {
  flex: 1;
  font-size: 1.1rem;
  line-height: 1.6;
}

.greeting {
  font-size: 1.3rem;
  font-weight: 600;
  color: var(--color-secondary);
  margin-bottom: 15px;
}

.cta-container {
  margin-top: 25px;
  text-align: center;
}


/* Featured Spells Section */
.featured-section {
  margin: 20px 0;
}

.section-title {
  font-size: 1.8rem;
  color: var(--color-secondary);
  text-align: center;
  margin-bottom: 25px;
  position: relative;
}

.section-title::after {
  content: "";
  position: absolute;
  bottom: -10px;
  left: 50%;
  transform: translateX(-50%);
  width: 100px;
  height: 3px;
  background: var(--color-secondary);
}

.featured-spells {
  display: flex;
  gap: 20px;
  justify-content: center;
  flex-wrap: wrap;
}

.featured-spell {
  flex: 1;
  min-width: 250px;
  max-width: 350px;
  background-color: rgba(255, 255, 255, 0.1);
  border: 1px solid var(--color-border);
  border-radius: 8px;
  padding: 20px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  transition: all 0.3s ease;
}

.featured-spell:hover {
  transform: translateY(-5px);
  box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
  border-color: var(--color-primary);
}

.spell-name {
  font-size: 1.4rem;
  color: var(--color-primary);
  margin: 0 0 10px;
  text-align: center;
}

.spell-meta {
  display: flex;
  justify-content: center;
  gap: 10px;
  margin-bottom: 15px;
}

.spell-level {
  background-color: var(--color-accent);
  color: var(--color-background);
  padding: 3px 10px;
  border-radius: 12px;
  font-size: 0.8rem;
}

.spell-excerpt {
  font-size: 0.95rem;
  line-height: 1.5;
  margin-bottom: 20px;
  text-align: justify;
  min-height: 70px;
}

.details-link {
  display: block;
  width: 100%;
  background-color: transparent;
  color: var(--color-secondary);
  border: 1px solid var(--color-secondary);
  padding: 8px 0;
  border-radius: 4px;
  text-align: center;
  font-family: 'Cinzel', serif;
  cursor: pointer;
  transition: all 0.3s ease;
}

.details-link:hover {
  background-color: var(--color-secondary);
  color: var(--color-background);
}

/* Lore Section */
.lore-section {
  padding: 20px 40px;
  margin-top: 20px;
}

.parchment {
  background-color: rgba(255, 248, 220, 0.7);
  padding: 30px;
  border-radius: 8px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  position: relative;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='100' height='100' opacity='0.05'%3E%3Cpath d='M11 18c3.866 0 7-3.134 7-7s-3.134-7-7-7-7 3.134-7 7 3.134 7 7 7zm48 25c3.866 0 7-3.134 7-7s-3.134-7-7-7-7 3.134-7 7 3.134 7 7 7zm-43-7c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zm63 31c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zM34 90c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zm56-76c1.657 0 3-1.343 3-3s-1.343-3-3-3-3 1.343-3 3 1.343 3 3 3zM12 86c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm28-65c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm23-11c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm-6 60c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm29 22c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zM32 63c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm57-13c2.76 0 5-2.24 5-5s-2.24-5-5-5-5 2.24-5 5 2.24 5 5 5zm-9-21c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM60 91c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM35 41c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2zM12 60c1.105 0 2-.895 2-2s-.895-2-2-2-2 .895-2 2 .895 2 2 2z' fill='%23000000' fill-opacity='0.1' fill-rule='evenodd'/%3E%3C/svg%3E");
}

.parchment::before,
.parchment::after {
  content: "";
  position: absolute;
  width: 40px;
  height: 40px;
  opacity: 0.4;
  background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 50 50' fill='%238b0000'%3E%3Cpath d='M25 0C11.193 0 0 11.193 0 25s11.193 25 25 25 25-11.193 25-25S38.807 0 25 0zm0 5c11.046 0 20 8.954 20 20s-8.954 20-20 20S5 36.046 5 25 13.954 5 25 5zm0 5a15 15 0 00-15 15 15 15 0 0015 15 15 15 0 0015-15 15 15 0 00-15-15z'/%3E%3C/svg%3E");
  background-size: contain;
  background-repeat: no-repeat;
}

.parchment::before {
  top: 15px;
  left: 15px;
}

.parchment::after {
  bottom: 15px;
  right: 15px;
}

.lore-title {
  text-align: center;
  font-size: 1.5rem;
  color: var(--color-primary);
  margin-bottom: 20px;
}

.lore-text {
  font-style: italic;
  text-align: center;
  line-height: 1.8;
  font-size: 1.1rem;
  max-width: 800px;
  margin: 0 auto;
}

.signature {
  text-align: right;
  margin-top: 15px;
  font-family: 'Tangerine', cursive;
  font-size: 1.5rem;
  color: var(--color-secondary);
}

/* Responsive adjustments */
@media (max-width: 900px) {
  .main-title {
    font-size: 2.5rem;
  }

  .intro-section {
    flex-direction: column;
    text-align: center;
  }

  .featured-spells {
    flex-direction: column;
    align-items: center;
  }

  .featured-spell {
    max-width: 100%;
  }
}

@media (max-width: 600px) {
  .main-title {
    font-size: 2rem;
  }

  .subtitle {
    font-size: 1.2rem;
  }

  .grimoire-decoration {
    width: 20px;
  }

  .grimoire-pages {
    padding: 20px 15px;
  }
}
</style>