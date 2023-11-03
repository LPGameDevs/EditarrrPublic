import { BadRequestException } from "../utils.mjs";

export class RatingsApi {
    constructor(ratingsDbClient, levelsDbClient) {
        this.ratingsDbClient = ratingsDbClient;
        this.levelsDbClient = levelsDbClient;
    }

    async postRating(levelId, requestJSON) {
        var rating = requestJSON.rating;
        if (!rating) throw new BadRequestException(`'rating' must be provided in the request.`);
        var ratingLevelName = requestJSON.code;
        if (!ratingLevelName) throw new BadRequestException(`'code' must be provided in the request.`);
        var ratingCreatorId = requestJSON.creator;
        if (!ratingCreatorId) throw new BadRequestException(`'creator' must be provided in the request.`);
        var ratingCreatorName = requestJSON.creatorName;
        if (!ratingCreatorName) throw new BadRequestException(`'creatorName' must be provided in the request.`);

        var level = await this.levelsDbClient.getLevel(levelId);
        if (!level) {
            throw new BadRequestException(`level ${levelId} does not exist`)
        }

        var generatedRatingId = await this.ratingsDbClient.putRating(
            rating, 
            levelId,
            ratingLevelName,
            ratingCreatorId,
            ratingCreatorName);

        var allRatingsForLevel = await this.ratingsDbClient.getAllRatingsForLevel(levelId);
        
        var totalNumberOfRatings = 0; 
        var sumOfAllRatings = 0.0;
        for (let i = 0; i < allRatingsForLevel.length; i++) {
            var dbRating = allRatingsForLevel[i];

            totalNumberOfRatings++;
            sumOfAllRatings += parseFloat(dbRating.rating);
        }
        var avgRating = sumOfAllRatings / allRatingsForLevel.length;
        
        await this.levelsDbClient.updateLevelRatingData(levelId, avgRating, totalNumberOfRatings);

        return {
            "message": `Success! Created rating for: ${ratingLevelName}`,
            "id": generatedRatingId,
        }
    }
}