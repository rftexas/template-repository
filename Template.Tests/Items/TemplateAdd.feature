Feature: Template Add

    Rule: Adding templates
        Scenario: Add a new template
            Given I have a valid template payload
            And the template does not already exist
            When I send a request to add the template
            Then the template should be successfully added

        Scenario: Add a template with invalid payload
            Given I have an invalid template payload
            And the template does not already exist
            When I send a request to add the template
            Then the template should not be added

        Scenario: Add a template that already exists
            Given I have a valid template payload
            And the template already exists
            When I send a request to add the template
            Then the template should not be added